// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using jsc.commons.config;
using jsc.commons.hierarchy.acl;
using jsc.commons.hierarchy.acl.interfaces;
using jsc.commons.hierarchy.acl.privileges;
using jsc.commons.hierarchy.acl.privileges.interfaces;
using jsc.commons.hierarchy.config;
using jsc.commons.hierarchy.exceptions;
using jsc.commons.hierarchy.groups;
using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.hierarchy.users;
using jsc.commons.misc;

using Enumerable = System.Linq.Enumerable;

namespace jsc.commons.hierarchy {

   public class HierarchyManagerAsync : IHierarchyManagerAsync {

      private readonly IHierarchyManagerConfiguration _conf;

      public HierarchyManagerAsync(
            IHierarchyAsync hierarchy,
            IHierarchyManagerConfiguration hierarchyManagerConfiguration = null ) {
         hierarchy.MustNotBeNull( nameof( hierarchy ) );
         if( hierarchyManagerConfiguration != null ) {
            hierarchyManagerConfiguration.BaseFolder.MustNotBeNull(
                  nameof( hierarchyManagerConfiguration.BaseFolder ) );
            hierarchyManagerConfiguration.UsersFolder.MustNotBeNull(
                  nameof( hierarchyManagerConfiguration.UsersFolder ) );
            hierarchyManagerConfiguration.GroupsFolder.MustNotBeNull(
                  nameof( hierarchyManagerConfiguration.GroupsFolder ) );
            hierarchyManagerConfiguration.ExcludePaths ??= new List<IPath>( 0 );
         }

         Hierarchy = hierarchy;
         _conf ??= Config.New<IHierarchyManagerConfiguration>( );

         Hierarchy.ResourceCreated += OnHierarchyResourceCreated;
         Hierarchy.ResourceDeleted += OnHierarchyResourceDeleted;
         Hierarchy.ResourceMoved += OnHierarchyResourceMoved;

         BootStrap( ).Wait( ); // timeout?
      }

      private IPath BaseFolderPath => _conf.BaseFolder;

      private IPath UsersFolderPath => BaseFolderPath.Append( _conf.UsersFolder );

      private IPath GroupsFolderPath => BaseFolderPath.Append( _conf.GroupsFolder );

      public IHierarchyAsync Hierarchy { get; }

      public async Task<User> GetSystemUserAsync( ) {
         return await Hierarchy.GetAsync<User>( UsersFolderPath.Append( _conf.SystemUser ) );
      }

      public async Task<T> GetAsync<T>( User user, IPath path ) where T : IResource {
         CheckResponsibility( path );
         await CheckPrivilege( user, path, ReadPrivilege.Instance );

         return await Hierarchy.GetAsync<T>( path );
      }

      public async Task SetAsync( User user, IResource resource ) {
         CheckResponsibility( resource.Path );
         await CheckPrivilege( user, resource.Path.BasePath, CreatePrivilege.Instance );

         await Hierarchy.SetAsync( resource );
      }

      public async Task DeleteAsync( User user, IResource resource ) {
         CheckResponsibility( resource.Path );
         await CheckPrivilege( user, resource.Path, DeletePrivilege.Instance );

         await Hierarchy.DeleteAsync( resource );
      }

      public async Task<IEnumerable<string>> GetChildrenResourceNamesAsync( User user, IPath path ) {
         CheckResponsibility( path );
         await CheckPrivilege( user, path, ReadPrivilege.Instance );

         return await Hierarchy.GetChildrenResourceNamesAsync( path );
      }

      public async Task MoveAsync( User user, IResource resource, IPath targetPath ) {
         CheckResponsibility( resource.Path );
         CheckResponsibility( targetPath );
         await CheckPrivilege( user, resource.Path, DeletePrivilege.Instance );
         await CheckPrivilege( user, targetPath, CreatePrivilege.Instance ); // write privilege?

         await Hierarchy.MoveAsync( resource, targetPath );
      }

      public async Task<IEnumerable<Group>> GetGroupsForUserAsync( User user ) {
         List<Group> groups = new List<Group>( );

         await foreach( Group group in user.GetGroupsAsync( Hierarchy ) ) {
            groups.Add( group );
            groups.AddRange( await GetParentGroupsAsync( group ) );
         }

         return groups;
      }

      public async Task<bool> HasPrivilegeAsync( User user, IPrivilege privilege, IPath path ) {
         IList<IPath> groupPaths = ( await GetGroupsForUserAsync( user ) ).Select( g => g.Path ).ToList( );
         do {
            IResource resource = await Hierarchy.GetAsync<IResource>( path );
            IAccessControlList acl = resource.GetAccessControlList( Hierarchy.Configuration );
            bool? hasPrivilege = acl.HasPrivilege( user.Path, groupPaths, privilege );

            if( hasPrivilege.HasValue ) // if null, the ACL check is not conclusive
               return hasPrivilege.Value;

            if( !acl.AccessControlRules.Any( ) // on empty, Inherit is assumed
                  // if the first ACR is explicitly not Inherit, we're done: disallow by default
                  &&acl.AccessControlRules.First( ).Action != EnAccessControlAction.Inherit )
               return false;

            path = path.BasePath;
         } while( BaseFolderPath.IsContainedIn( path ) );

         return false;
      }

      private void CheckResponsibility( IPath path ) {
         if( BaseFolderPath.IsContainedIn( path ) )
            throw new PathOutsideOfBoundsException(
                  this,
                  path,
                  $"base path {BaseFolderPath}",
                  BaseFolderPath,
                  _conf.ExcludePaths );

         foreach( IPath excludedPath in _conf.ExcludePaths )
            if( excludedPath.IsContainedIn( path ) )
               throw new PathOutsideOfBoundsException(
                     this,
                     path,
                     $"excluded path {excludedPath}",
                     BaseFolderPath,
                     _conf.ExcludePaths );
      }

      private async Task CheckPrivilege( User user, IPath path, IPrivilege privilege ) {
         if( !await HasPrivilegeAsync( user, privilege, path ) )
            throw new InsufficientPrivilegesException( user, path, privilege );
      }

      private async Task<IEnumerable<Group>> GetParentGroupsAsync( Group group ) {
         IPath parentPath = group.Path.BasePath;
         if( parentPath == Path.RootPath )
            return Enumerable.Empty<Group>( );

         IResource parentResource = await Hierarchy.GetAsync<IResource>( parentPath );
         if( !( parentResource is Group parentGroup ) )
            return Enumerable.Empty<Group>( );

         return new[] {parentGroup}.Union( await GetParentGroupsAsync( parentGroup ) );
      }

      private Task OnHierarchyResourceMoved( object sender, ResourceMovedEventArgs args ) {
         throw new NotImplementedException( );
      }

      private Task OnHierarchyResourceDeleted( object sender, ResourceDeletedEventArgs args ) {
         throw new NotImplementedException( );
      }

      private Task OnHierarchyResourceCreated( object sender, ResourceSetEventArgs args ) {
         throw new NotImplementedException( );
      }

      private async Task BootStrap( ) {
         try {
            Folder baseFolder = await GetOrCreateFolderRecursive( BaseFolderPath );
            await GetOrCreateFolderRecursive( UsersFolderPath );
            await GetOrCreateFolderRecursive( GroupsFolderPath );

            User systemUser = await GetOrCreateSystemUser( );

            try {
               IAccessControlList acl = baseFolder.GetAccessControlList( Hierarchy.Configuration );
               if( !acl.AccessControlRules.Any( ) ) {
                  if( _conf.BaseFolderAclFactory != null ) {
                     _conf.BaseFolderAclFactory( acl, systemUser );
                  } else {
                     acl.Add( AcrBuilder.ToEveryone( ).Deny( new AllPrivilege( ) ) );
                     acl.Add( AcrBuilder.To( systemUser ).Allow( new AllPrivilege( ) ) );
                  }

                  baseFolder.SetAccessControlList( acl );
                  await Hierarchy.SetAsync( baseFolder );
               }
            } catch( Exception exc ) {
               throw new Exception( $"failed to set ACL for base folder: {exc.Message}", exc );
            }
         } catch( Exception exc ) {
            throw new Exception( $"failed to bootstrap hierarchy in base folder {BaseFolderPath}: {exc.Message}", exc );
         }
      }

      private async Task<User> GetOrCreateSystemUser( ) {
         User systemUser;
         try {
            systemUser = await GetSystemUserAsync( );
            if( systemUser == null )
               throw new Exception( );
         } catch( Exception exc ) {
            systemUser = new User( UsersFolderPath, _conf.SystemUser );
            try {
               await Hierarchy.SetAsync( systemUser );
            } catch( Exception exc2 ) {
               throw new Exception(
                     $"failed to get or create system user: {exc2.Message}",
                     new AggregateException( exc, exc2 ) );
            }
         }

         return systemUser;
      }


      private async Task<Folder> GetOrCreateFolderRecursive( IPath path ) {
         path.MustNotBeNull( nameof( path ) );

         IPath currentPath = new Path( true );
         Folder folder = null;
         foreach( string pathElement in path.Elements ) {
            IPath previousPath = currentPath;
            currentPath = currentPath.Append( pathElement );
            try {
               folder = await Hierarchy.GetAsync<Folder>( currentPath );
               if( folder == null )
                  throw new Exception( $"folder {currentPath} does not exist" );
            } catch( Exception exc ) {
               folder = new Folder( previousPath, pathElement );
               try {
                  await Hierarchy.SetAsync( folder );
               } catch( Exception exc2 ) {
                  throw new Exception(
                        $"failed to get or create folder {path} recursively due to errors: {exc2.Message}",
                        new AggregateException( exc, exc2 ) );
               }
            }
         }

         return folder;
      }

   }

}
