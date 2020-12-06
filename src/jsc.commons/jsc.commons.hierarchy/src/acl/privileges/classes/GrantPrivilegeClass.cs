// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.acl.privileges.interfaces;

namespace jsc.commons.hierarchy.acl.privileges.classes {

   public class GrantPrivilegeClass : PrivilegeClassBase {

      public GrantPrivilegeClass( ) : base( "grant" ) { }

      public static GrantPrivilegeClass Instance { get; } = new GrantPrivilegeClass( );

      public override IPrivilege CreatePrivilege( ) {
         return new GrantPrivilege( );
      }

   }

}