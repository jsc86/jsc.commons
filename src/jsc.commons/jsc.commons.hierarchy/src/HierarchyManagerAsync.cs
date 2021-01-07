// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using jsc.commons.behaving;
using jsc.commons.behaving.interfaces;
using jsc.commons.config;
using jsc.commons.hierarchy.acl;
using jsc.commons.hierarchy.acl.interfaces;
using jsc.commons.hierarchy.acl.privileges;
using jsc.commons.hierarchy.acl.privileges.interfaces;
using jsc.commons.hierarchy.config;
using jsc.commons.hierarchy.exceptions;
using jsc.commons.hierarchy.groups;
using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.meta;
using jsc.commons.hierarchy.meta.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.resources;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.hierarchy.users;
using jsc.commons.misc;

using Enumerable = System.Linq.Enumerable;

namespace jsc.commons.hierarchy {

   public class HierarchyManagerAsync : IHierarchyManagerAsync {

      public const string TraceModule = "HIER_MGR";

      public const string TraceActionGetSysUser = "GSU";
      public const string TraceActionCheckResponsibility = "RES";
      public const string TraceActionCheckPrivileges = "ACL";
      public const string TraceActionBootStrap = "BST";

      private readonly TraceHandler _traceHandler;

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
         Configuration = hierarchyManagerConfiguration??Config.New<IHierarchyManagerConfiguration>( );

         _traceHandler = Configuration.TraceHandler;

         HierarchyAsync.ResourceSet += OnHierarchyResourceSet;
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
         _traceHandler?.Invoke( TraceModule, TraceActionGetSysUser, Trace.HintBegin );
         try {
            User systemUser = await HierarchyAsync.GetAsync<User>( UsersFolderPath.Append( Configuration.SystemUser ) );

            _traceHandler?.Invoke( TraceModule, TraceActionGetSysUser, Trace.HintEnd );

            return systemUser;
         } catch( Exception ) {
            _traceHandler?.Invoke( TraceModule, TraceActionGetSysUser, Trace.HintError );
            throw;
         }
      }

      public async Task<T> GetAsync<T>( User user, Path path ) where T : IResource {
         _traceHandler?.Invoke( TraceModule, Trace.ActionGet, Trace.HintBegin, user.Path, path );

         try {
            CheckResponsibility( path );
            await CheckPrivilege( user, path, ReadPrivilege.Instance );

            T resource = await HierarchyAsync.GetAsync<T>( path );

            _traceHandler?.Invoke( TraceModule, Trace.ActionGet, Trace.HintEnd, user.Path, path );

            return resource;
         } catch( Exception ) {
            _traceHandler?.Invoke( TraceModule, Trace.ActionGet, Trace.HintError, user.Path, path );
            throw;
         }
      }

      public async Task SetAsync( User user, IResource resource ) {
         _traceHandler?.Invoke( TraceModule, Trace.ActionSet, Trace.HintBegin, user.Path, resource?.Path );

         try {
            CheckResponsibility( resource.Path );
            await CheckPrivilege( user, resource.Path.BasePath, CreatePrivilege.Instance );

            await HierarchyAsync.SetAsync( resource );

            _traceHandler?.Invoke( TraceModule, Trace.ActionSet, Trace.HintEnd, user.Path, resource.Path );
         } catch( Exception ) {
            _traceHandler?.Invoke( TraceModule, Trace.ActionSet, Trace.HintError, user.Path, resource?.Path );
            throw;
         }
      }

      public async Task DeleteAsync( User user, IResource resource ) {
         _traceHandler?.Invoke( TraceModule, Trace.ActionDelete, Trace.HintBegin, user.Path, resource?.Path );

         try {
            CheckResponsibility( resource.Path );
            await CheckPrivilege( user, resource.Path, DeletePrivilege.Instance );

            await HierarchyAsync.DeleteAsync( resource );

            _traceHandler?.Invoke( TraceModule, Trace.ActionDelete, Trace.HintEnd, user.Path, resource?.Path );
         } catch( Exception ) {
            _traceHandler?.Invoke( TraceModule, Trace.ActionDelete, Trace.HintError, user.Path, resource?.Path );
            throw;
         }
      }

      public async Task<IEnumerable<string>> GetChildrenResourceNamesAsync( User user, Path path ) {
         _traceHandler?.Invoke( TraceModule, Trace.ActionList, Trace.HintBegin, user.Path, path );

         try {
            CheckResponsibility( path );
            await CheckPrivilege( user, path, ReadPrivilege.Instance );

            IEnumerable<string> list = await HierarchyAsync.GetChildrenResourceNamesAsync( path );

            _traceHandler?.Invoke( TraceModule, Trace.ActionList, Trace.HintEnd, user.Path, path );

            return list;
         } catch( Exception ) {
            _traceHandler?.Invoke( TraceModule, Trace.ActionList, Trace.HintError, user.Path, path );
            throw;
         }
      }

      public async Task MoveAsync( User user, IResource resource, Path targetPath ) {
         _traceHandler?.Invoke( TraceModule, Trace.ActionMove, Trace.HintBegin, user.Path, resource?.Path, targetPath );

         try {
            CheckResponsibility( resource.Path );
            CheckResponsibility( targetPath );
            await CheckPrivilege( user, resource.Path, DeletePrivilege.Instance );
            await CheckPrivilege( user, targetPath, CreatePrivilege.Instance ); // write privilege?

            await HierarchyAsync.MoveAsync( resource, targetPath );

            _traceHandler?.Invoke( TraceModule, Trace.ActionMove, Trace.HintEnd, user.Path, resource.Path, targetPath );
         } catch( Exception ) {
            _traceHandler?.Invoke(
                  TraceModule,
                  Trace.ActionMove,
                  Trace.HintError,
                  user.Path,
                  resource?.Path,
                  targetPath );
            throw;
         }
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
         _traceHandler?.Invoke( TraceModule, TraceActionCheckPrivileges, Trace.HintBegin, user?.Path, path );

         try {
            IList<Path> groupPaths = ( await GetGroupsForUserAsync( user ) ).Select( g => g.Path ).ToList( );
            do {
               IResource resource = await HierarchyAsync.GetAsync<IResource>( path );
               IAccessControlList acl = resource.GetAccessControlList( HierarchyAsync.Configuration );
               bool? hasPrivilege = acl.HasPrivilege( user.Path, groupPaths, privilege );

               if( hasPrivilege.HasValue ) { // if null, the ACL check is not conclusive
                  _traceHandler?.Invoke( TraceModule, TraceActionCheckPrivileges, Trace.HintEnd, user.Path, path );
                  return hasPrivilege.Value;
               }

               if( acl.AccessControlRules.Any( ) // on empty, Inherit is assumed
                     // if the first ACR is explicitly not Inherit, we're done: disallow by default
                     &&acl.AccessControlRules.First( ).Action != EnAccessControlAction.Inherit ) {
                  _traceHandler?.Invoke( TraceModule, TraceActionCheckPrivileges, Trace.HintEnd, user.Path, path );
                  return false;
               }

               path = path.BasePath;
            } while( path.IsContainedIn( BaseFolderPath )
                  ||BaseFolderPath.Equals( path ) );

            _traceHandler?.Invoke( TraceModule, TraceActionCheckPrivileges, Trace.HintEnd, user.Path, path );
            return false;
         } catch( Exception ) {
            _traceHandler?.Invoke( TraceModule, TraceActionCheckPrivileges, Trace.HintError, user?.Path, path );
            throw;
         }
      }

      private void CheckResponsibility( Path path, bool throwException = true ) {
         _traceHandler?.Invoke( TraceModule, TraceActionCheckResponsibility, Trace.HintBegin, path );

         try {
            if( BaseFolderPath.IsContainedIn( path ) ) {
               if( throwException )
                  throw new PathOutsideOfBoundsException(
                        this,
                        path,
                        $"base path {BaseFolderPath}",
                        BaseFolderPath,
                        Configuration.ExcludePaths );

               _traceHandler?.Invoke( TraceModule, TraceActionCheckResponsibility, Trace.HintEnd, path );
               return;
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

                  _traceHandler?.Invoke( TraceModule, TraceActionCheckResponsibility, Trace.HintEnd, path );
                  return;
               }

            _traceHandler?.Invoke( TraceModule, TraceActionCheckResponsibility, Trace.HintEnd, path );
         } catch( Exception ) {
            _traceHandler?.Invoke( TraceModule, TraceActionCheckResponsibility, Trace.HintError, path );
            throw;
         }
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

      private async Task OnHierarchyResourceMoved( object sender, ResourceMovedEventArgs args ) {
         foreach( IBehavior meta in args.Resource.Meta.Objects( ) )
            if( meta.TryGetAutoModHandler( out IMetaAutoModHandler autoModHandler ) )
               await autoModHandler.OnMove( args, this );
      }

      private async Task OnHierarchyResourceDeleted( object sender, ResourceDeletedEventArgs args ) {
         foreach( IBehavior meta in args.Resource.Meta.Objects( ) )
            if( meta.TryGetAutoModHandler( out IMetaAutoModHandler autoModHandler ) )
               await autoModHandler.OnDelete( args, this );
      }

      private async Task OnHierarchyResourceSet( object sender, ResourceSetEventArgs args ) {
         foreach( IBehavior meta in args.Resource.Meta.Objects( ) )
            if( meta.TryGetAutoModHandler( out IMetaAutoModHandler autoModHandler ) )
               await autoModHandler.OnSet( args, this );
      }

      private async Task BootStrap( ) {
         _traceHandler?.Invoke( TraceModule, TraceActionBootStrap, Trace.HintBegin );
         try {
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
               throw new Exception(
                     $"failed to bootstrap hierarchy in base folder {BaseFolderPath}: {exc.Message}",
                     exc );
            }

            _traceHandler?.Invoke( TraceModule, TraceActionBootStrap, Trace.HintEnd );
         } catch( Exception ) {
            _traceHandler?.Invoke( TraceModule, TraceActionBootStrap, Trace.HintError );
            throw;
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
