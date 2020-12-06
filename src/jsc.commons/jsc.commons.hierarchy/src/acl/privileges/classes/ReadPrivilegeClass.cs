// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.acl.privileges.interfaces;

namespace jsc.commons.hierarchy.acl.privileges.classes {

   public class ReadPrivilegeClass : PrivilegeClassBase {

      public ReadPrivilegeClass( ) : base( "read" ) { }

      public static ReadPrivilegeClass Instance { get; } = new ReadPrivilegeClass( );

      public override IPrivilege CreatePrivilege( ) {
         return new ReadPrivilege( );
      }

   }

}