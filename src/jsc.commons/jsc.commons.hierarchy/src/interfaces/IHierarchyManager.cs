// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.hierarchy.acl.privileges.interfaces;
using jsc.commons.hierarchy.groups;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.hierarchy.users;

namespace jsc.commons.hierarchy.interfaces {

   public interface IHierarchyManager : IHierarchyManagerAsync {

      IHierarchy Hierarchy { get; }

      T Get<T>( User user, IPath path ) where T : IResource;

      bool TryGet<T>( User user, IPath path, out T resource ) where T : IResource;

      void Set( User user, IResource resource );

      void Delete( User user, IResource resource );

      IEnumerable<string> GetChildrenResourceNames( User user, IPath path );

      void Move( User user, IResource resource, IPath targetPath );

      bool HasPrivilege( User user, IPrivilege privilege, IPath path );

      User GetSystemUser( );

      IEnumerable<Group> GetGroupsForUser( User user );

   }

}