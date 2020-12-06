// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.acl.interfaces;
using jsc.commons.hierarchy.acl.privileges.interfaces;
using jsc.commons.hierarchy.groups;
using jsc.commons.hierarchy.users;

namespace jsc.commons.hierarchy.acl {

   public class AcrBuilder {

      public static AcrInherit Inherit( ) {
         return new AcrInherit( );
      }


      public static AcrUsrBuilder To( User user ) {
         return new AcrUsrBuilder( user );
      }


      public static AcrGrpBuilder To( Group group ) {
         return new AcrGrpBuilder( group );
      }

      public static AcrEveryoneBuilder ToEveryone( ) {
         return new AcrEveryoneBuilder( );
      }

   }

   public class AcrEveryoneBuilder {

      public AcrEveryone Allow( params IPrivilege[] privileges ) {
         return new AcrEveryone( EnAccessControlAction.Allow, privileges );
      }

      public AcrEveryone Deny( params IPrivilege[] privileges ) {
         return new AcrEveryone( EnAccessControlAction.Deny, privileges );
      }

   }

   public class AcrUsrBuilder {

      public AcrUsrBuilder( User user ) {
         User = user;
      }

      private User User { get; }

      public AcrUser Allow( params IPrivilege[] privileges ) {
         return new AcrUser( EnAccessControlAction.Allow, User.Path, privileges );
      }


      public AcrUser Deny( params IPrivilege[] privileges ) {
         return new AcrUser( EnAccessControlAction.Deny, User.Path, privileges );
      }

   }

   public class AcrGrpBuilder {

      public AcrGrpBuilder( Group group ) {
         Group = group;
      }

      private Group Group { get; }

      public AcrGroup Allow( params IPrivilege[] privileges ) {
         return new AcrGroup( EnAccessControlAction.Allow, Group.Path, privileges );
      }


      public AcrGroup Deny( params IPrivilege[] privileges ) {
         return new AcrGroup( EnAccessControlAction.Deny, Group.Path, privileges );
      }

   }

}