// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.acl.privileges.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.users;

namespace jsc.commons.hierarchy.exceptions {

   public class InsufficientPrivilegesException : HierarchyManagerException {

      public InsufficientPrivilegesException( User user, Path path, IPrivilege privilege ) : base(
            $"user {user.Name} does not have privilege {privilege} in {path}" ) {
         User = user;
         Path = path;
         Privilege = privilege;
      }

      public User User { get; }
      public Path Path { get; }
      public IPrivilege Privilege { get; }

   }

}