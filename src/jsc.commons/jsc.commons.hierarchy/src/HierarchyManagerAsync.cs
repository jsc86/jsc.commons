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
using jsc.commons.hierarchy.resources;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.hierarchy.users;
using jsc.commons.misc;

using Enumerable = System.Linq.Enumerable;

namespace jsc.commons.hierarchy {

   public class HierarchyManagerAsync : IHierarchyManagerAsync {

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
            hierarchyManagerConfiguration.SystemUser.MustNotBeNull(
                  nameof( hierarchyManagerConfiguration.SystemUser ) );
            hierarchyManagerConfiguration.ExcludePaths ??= new List<Path>( 0 );
         }

         HierarchyAsync = hierarchy;
         Configuration ??= Config.New<IHierarchyManagerConfiguration>( );

         HierarchyAsync.ResourceCreated += OnHierarchyResourceCreated;
         HierarchyAsync.ResourceDeleted += OnHierarchyResourceDeleted;
         HierarchyAsync.ResourceMoved += OnHierarchyResourceMoved;

         BootStrap( ).Wait( ); // timeout?
      }

      public Path BaseFolderPath => Configuration.BaseFolder;

      public Path UsersFolderPath => BaseFolderPath.Append( Configuration.UsersFolder );

      public Path GroupsFolderPath => BaseFolderPath.Append( Configuration.GroupsFolder );

      public IHierarchyAsync HierarchyAsync { get; }

      public IHierarchyManagerConfiguration Configuration { get; }

      public async Task<User> GetSystemUserAsync( ) {
         return await HierarchyAsync.GetAsync<User>( UsersFolderPath.Append( Configuration.SystemUser ) );
      }

      public async Task<T> GetAsync<T>( User user, Path path ) where T : IResource {
         CheckResponsibility( path );
         await CheckPrivilege( user, path, ReadPrivilege.Instance );

         return await HierarchyAsync.GetAsync<T>( path );
      }

      public async Task SetAsync( User user, IResource resource ) {
         CheckResponsibility( resource.Path );
         await CheckPrivilege( user, resource.Path.BasePath, CreatePrivilege.Instance );

         await HierarchyAsync.SetAsync( resource );
      }

      public async Task DeleteAsync( User user, IResource resource ) {
         CheckResponsibility( resource.Path );
         await CheckPrivilege( user, resource.Path, DeletePrivilege.Instance );

         await HierarchyAsync.DeleteAsync( resource );
      }

      public async Task<IEnumerable<string>> GetChildrenResourceNamesAsync( User user, Path path ) {
         CheckResponsibility( path );
         await CheckPrivilege( user, path, ReadPrivilege.Instance );

         return await HierarchyAsync.GetChildrenResourceNamesAsync( path );
      }

      public async Task MoveAsync( User user, IResource resource, Path targetPath ) {
         CheckResponsibility( resource.Path );
         CheckResponsibility( targetPath );
         await CheckPrivilege( user, resource.Path, DeletePrivilege.Instance );
         await CheckPrivilege( user, targetPath, CreatePrivilege.Instance ); // write privilege?

         await HierarchyAsync.MoveAsync( resource, targetPath );
      }

      public async Task<IEnumerable<Group>> GetGroupsForUserAsync( User user ) {
         List<Group> groups = new List<Group>( );

         foreach( Group group in await user.GetGroupsAsync( HierarchyAsync ) ) {
            groups.Add( group );
            groups.AddRange( await GetParentGroupsAsync( group ) );
         }

         return groups;
      }

      public async Task<bool> HasPrivilegeAsync( User user, IPrivilege privilege, Path path ) {
         IList<Path> groupPaths = ( await GetGroupsForUserAsync( user ) ).Select( g => g.Path ).ToList( );
         do {
            IResource resource = await HierarchyAsync.GetAsync<IResource>( path );
            IAccessControlList acl = resource.GetAccessControlList( HierarchyAsync.Configuration );
            bool? hasPrivilege = acl.HasPrivilege( user.Path, groupPaths, privilege );

            if( hasPrivilege.HasValue ) // if null, the ACL check is not conclusive
               return hasPrivilege.Value;

            if( acl.AccessControlRules.Any( ) // on empty, Inherit is assumed
                  // if the first ACR is explicitly not Inherit, we're done: disallow by default
                  &&acl.AccessControlRules.First( ).Action != EnAccessControlAction.Inherit )
               return false;

            path = path.BasePath;
         } while( BaseFolderPath.IsContainedIn( path )
               ||BaseFolderPath.Equals( path ) );

         return false;
      }

      private bool CheckResponsibility( Path path, bool throwException = true ) {
         if( BaseFolderPath.IsContainedIn( path ) ) {
            if( throwException )
               throw new PathOutsideOfBoundsException(
                     this,
                     path,
                     $"base path {BaseFolderPath}",
                     BaseFolderPath,
                     Configuration.ExcludePaths );

            return false;
         }

         foreach( Path excludedPath in Configuration.ExcludePaths )
            if( excludedPath.IsContainedIn( path ) ) {
               if( throwException )
                  throw new PathOutsideOfBoundsException(
                        this,
                        path,
                        $"excluded path {excludedPath}",
                        BaseFolderPath,
                        Configuration.ExcludePaths );
               return false;
            }

         return true;
      }

      private async Task CheckPrivilege( User user, Path path, IPrivilege privilege ) {
         if( !await HasPrivilegeAsync( user, privilege, path ) )
            throw new InsufficientPrivilegesException( user, path, privilege );
      }

      private async Task<IEnumerable<Group>> GetParentGroupsAsync( Group group ) {
         Path parentPath = group.Path.BasePath;
         if( parentPath == Path.RootPath )
            return Enumerable.Empty<Group>( );

         IResource parentResource = await HierarchyAsync.GetAsync<IResource>( parentPath );
         if( !( parentResource is Group parentGroup ) )
            return Enumerable.Empty<Group>( );

         return new[] {parentGroup}.Union( await GetParentGroupsAsync( parentGroup ) );
      }

      private Task OnHierarchyResourceMoved( object sender, ResourceMovedEventArgs args ) {
         // TODO: implement
         return Task.CompletedTask;
      }

      private Task OnHierarchyResourceDeleted( object sender, ResourceDeletedEventArgs args ) {
         // TODO: implement
         return Task.CompletedTask;
      }

      private Task OnHierarchyResourceCreated( object sender, ResourceSetEventArgs args ) {
         // TODO: implement
         return Task.CompletedTask;
      }

      private async Task BootStrap( ) {
         try {
            Folder baseFolder = await GetOrCreateFolderRecursive( BaseFolderPath );
            await GetOrCreateFolderRecursive( UsersFolderPath );
            await GetOrCreateFolderRecursive( GroupsFolderPath );

            User systemUser = await GetOrCreateSystemUser( );

            try {
               IAccessControlList acl = baseFolder.GetAccessControlList( HierarchyAsync.Configuration );
               if( !acl.AccessControlRules.Any( ) ) {
                  if( Configuration.BaseFolderAclFactory != null ) {
                     Configuration.BaseFolderAclFactory( acl, systemUser );
                  } else {
                     acl.Add( AcrBuilder.ToEveryone( ).Deny( new AllPrivilege( ) ) );
                     acl.Add( AcrBuilder.To( systemUser ).Allow( new AllPrivilege( ) ) );
                  }

                  baseFolder.SetAccessControlList( acl );
                  await HierarchyAsync.SetAsync( baseFolder );
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
            systemUser = new User( UsersFolderPath, Configuration.SystemUser );
            try {
               await HierarchyAsync.SetAsync( systemUser );
            } catch( Exception exc2 ) {
               throw new Exception(
                     $"failed to get or create system user: {exc2.Message}",
                     new AggregateException( exc, exc2 ) );
            }
         }

         return systemUser;
      }


      private async Task<Folder> GetOrCreateFolderRecursive( Path path ) {
         path.MustNotBeNull( nameof( path ) );

         if( path.Equals( Path.RootPath ) ) {
            Folder rootFolder;
            try {
               rootFolder = await HierarchyAsync.GetAsync<Folder>( Path.RootPath );
               if( rootFolder == null )
                  throw new Exception( "did not find root folder" );
            } catch( Exception exc ) {
               rootFolder = new Folder( Path.RootPath, null );
               try {
                  await HierarchyAsync.SetAsync( rootFolder );
                  rootFolder = await HierarchyAsync.GetAsync<Folder>( Path.RootPath );
               } catch( Exception exc2 ) {
                  throw new Exception(
                        $"failed to get or create root folder due to errors: {exc2.Message}",
                        new AggregateException( exc, exc2 ) );
               }
            }

            return rootFolder;
         }

         Path currentPath = new Path( true );
         Folder folder = null;
         foreach( string pathElement in path.Elements ) {
            Path previousPath = currentPath;
            currentPath = currentPath.Append( pathElement );
            try {
               folder = await HierarchyAsync.GetAsync<Folder>( currentPath );
               if( folder == null )
                  throw new Exception( $"folder {currentPath} does not exist" );
            } catch( Exception exc ) {
               folder = new Folder( previousPath, pathElement );
               try {
                  await HierarchyAsync.SetAsync( folder );
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
