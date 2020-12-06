// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.acl.privileges.interfaces;

namespace jsc.commons.hierarchy.acl.privileges.classes {

   public class AllPrivilegeClass : PrivilegeClassBase {

      public AllPrivilegeClass( ) : base( "all" ) { }

      public static AllPrivilegeClass Instance { get; } = new AllPrivilegeClass( );

      public override IPrivilege CreatePrivilege( ) {
         return new AllPrivilege( );
      }

      public override string ToString( ) {
         return "ALL";
      }

   }

}