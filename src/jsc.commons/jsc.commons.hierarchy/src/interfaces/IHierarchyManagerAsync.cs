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
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.hierarchy.users;

namespace jsc.commons.hierarchy.interfaces {

   public interface IHierarchyManagerAsync {

      IHierarchyAsync HierarchyAsync { get; }

      IHierarchyManagerConfiguration Configuration { get; }

      Path BaseFolderPath { get; }

      Path UsersFolderPath { get; }

      Path GroupsFolderPath { get; }

      Task<User> GetSystemUserAsync( );

      Task<T> GetAsync<T>( User user, Path path ) where T : IResource;

      Task SetAsync( User user, IResource resource );

      Task DeleteAsync( User user, IResource resource );

      Task<IEnumerable<string>> GetChildrenResourceNamesAsync( User user, Path path );

      Task MoveAsync( User user, IResource resource, Path targetPath );

      Task<IEnumerable<Group>> GetGroupsForUserAsync( User user );

      Task<bool> HasPrivilegeAsync( User user, IPrivilege privilege, Path path );

   }

}