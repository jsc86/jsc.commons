// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.acl.privileges.classes;

namespace jsc.commons.hierarchy.acl.privileges {

   public class AllPrivilege : PrivilegeBase {

      public AllPrivilege( ) : base( AllPrivilegeClass.Instance ) { }

      public static AllPrivilege Instance { get; } = new AllPrivilege( );

   }

}