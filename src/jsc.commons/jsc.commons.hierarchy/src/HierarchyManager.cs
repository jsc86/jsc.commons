// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using jsc.commons.hierarchy.acl.privileges.interfaces;
using jsc.commons.hierarchy.config;
using jsc.commons.hierarchy.groups;
using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.hierarchy.users;

namespace jsc.commons.hierarchy {

   public class HierarchyManager : HierarchyManagerAsync, IHierarchyManager {

      public HierarchyManager(
            IHierarchy hierarchy,
            IHierarchyManagerConfiguration hierarchyManagerConfiguration = null ) : base(
            hierarchy,
            hierarchyManagerConfiguration ) {
         Hierarchy = hierarchy;
      }

      public IHierarchy Hierarchy { get; }

      public T Get<T>( User user, Path path ) where T : IResource {
         Task<T> getTask = GetAsync<T>( user, path );
         getTask.Wait( Hierarchy.Timeout );
         return getTask.Result;
      }

      public bool TryGet<T>( User user, Path path, out T resource ) where T : IResource {
         try {
            resource = Get<T>( user, path );
         } catch( Exception ) {
            resource = default;
            return false;
         }

         return true;
      }

      public void Set( User user, IResource resource ) {
         Task setTask = SetAsync( user, resource );
         setTask.Wait( Hierarchy.Timeout );
      }

      public void Delete( User user, IResource resource ) {
         Task deleteTask = DeleteAsync( user, resource );
         deleteTask.Wait( Hierarchy.Timeout );
      }

      public IEnumerable<string> GetChildrenResourceNames( User user, Path path ) {
         Task<IEnumerable<string>> getTask = GetChildrenResourceNamesAsync( user, path );
         getTask.Wait( Hierarchy.Timeout );
         return getTask.Result;
      }

      public void Move( User user, IResource resource, Path targetPath ) {
         Task moveTask = MoveAsync( user, resource, targetPath );
         moveTask.Wait( Hierarchy.Timeout );
      }

      public bool HasPrivilege( User user, IPrivilege privilege, Path path ) {
         Task<bool> hasPrivilegeTask = HasPrivilegeAsync( user, privilege, path );
         hasPrivilegeTask.Wait( Hierarchy.Timeout );
         return hasPrivilegeTask.Result;
      }

      public User GetSystemUser( ) {
         Task<User> getSystemUserTask = GetSystemUserAsync( );
         getSystemUserTask.Wait( Hierarchy.Timeout );
         return getSystemUserTask.Result;
      }

      public IEnumerable<Group> GetGroupsForUser( User user ) {
         Task<IEnumerable<Group>> getGroupsForUserTask = GetGroupsForUserAsync( user );
         getGroupsForUserTask.Wait( Hierarchy.Timeout );
         return getGroupsForUserTask.Result;
      }

   }

}
