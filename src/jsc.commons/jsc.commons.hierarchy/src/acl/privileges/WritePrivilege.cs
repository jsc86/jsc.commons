// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.acl.privileges.classes;

namespace jsc.commons.hierarchy.acl.privileges {

   public class WritePrivilege : PrivilegeBase {

      public WritePrivilege( ) : base( WritePrivilegeClass.Instance ) { }

      public static WritePrivilege Instance { get; } = new WritePrivilege( );

   }

}