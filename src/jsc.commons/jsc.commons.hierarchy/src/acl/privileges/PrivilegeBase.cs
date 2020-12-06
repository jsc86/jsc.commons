// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.acl.privileges.classes.interfaces;
using jsc.commons.hierarchy.acl.privileges.interfaces;
using jsc.commons.misc;

namespace jsc.commons.hierarchy.acl.privileges {

   public abstract class PrivilegeBase : IPrivilege {

      protected PrivilegeBase( IPrivilegeClass privilegeClass ) {
         privilegeClass.MustNotBeNull( nameof( privilegeClass ) );

         PrivilegeClass = privilegeClass;
      }

      public string Name => PrivilegeClass.Name;
      public IPrivilegeClass PrivilegeClass { get; }

      public override string ToString( ) {
         return PrivilegeClass.ToString( );
      }

   }

}