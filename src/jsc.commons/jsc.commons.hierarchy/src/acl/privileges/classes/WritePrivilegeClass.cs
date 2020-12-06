// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.acl.privileges.interfaces;

namespace jsc.commons.hierarchy.acl.privileges.classes {

   public class WritePrivilegeClass : PrivilegeClassBase {

      public WritePrivilegeClass( ) : base( "write" ) { }

      public static WritePrivilegeClass Instance { get; } = new WritePrivilegeClass( );

      public override IPrivilege CreatePrivilege( ) {
         return new WritePrivilege( );
      }

   }

}