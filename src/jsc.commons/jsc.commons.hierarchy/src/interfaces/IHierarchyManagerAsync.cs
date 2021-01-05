// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Threading.Tasks;

using jsc.commons.hierarchy.acl.privileges.interfaces;
using jsc.commons.hierarchy.config;
using jsc.commons.hierarchy.groups;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.hierarchy.users;

namespace jsc.commons.hierarchy.interfaces {

   public interface IHierarchyManagerAsync {

      IHierarchyAsync HierarchyAsync { get; }

      IHierarchyManagerConfiguration Configuration { get; }

      IPath BaseFolderPath { get; }

      IPath UsersFolderPath { get; }

      IPath GroupsFolderPath { get; }

      Task<User> GetSystemUserAsync( );

      Task<T> GetAsync<T>( User user, IPath path ) where T : IResource;

      Task SetAsync( User user, IResource resource );

      Task DeleteAsync( User user, IResource resource );

      Task<IEnumerable<string>> GetChildrenResourceNamesAsync( User user, IPath path );

      Task MoveAsync( User user, IResource resource, IPath targetPath );

      Task<IEnumerable<Group>> GetGroupsForUserAsync( User user );

      Task<bool> HasPrivilegeAsync( User user, IPrivilege privilege, IPath path );

   }

}