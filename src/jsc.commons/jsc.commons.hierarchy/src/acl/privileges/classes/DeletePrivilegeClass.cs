// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.acl.privileges.interfaces;

namespace jsc.commons.hierarchy.acl.privileges.classes {

   public class DeletePrivilegeClass : PrivilegeClassBase {

      public DeletePrivilegeClass( ) : base( "delete" ) { }

      public static DeletePrivilegeClass Instance { get; } = new DeletePrivilegeClass( );

      public override IPrivilege CreatePrivilege( ) {
         return new DeletePrivilege( );
      }

   }

}