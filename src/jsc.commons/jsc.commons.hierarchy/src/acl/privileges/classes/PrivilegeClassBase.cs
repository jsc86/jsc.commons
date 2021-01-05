// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.acl.privileges.classes.interfaces;
using jsc.commons.hierarchy.acl.privileges.interfaces;
using jsc.commons.misc;

namespace jsc.commons.hierarchy.acl.privileges.classes {

   public abstract class PrivilegeClassBase : IPrivilegeClass {

      protected PrivilegeClassBase( string name ) {
         name.MustNotBeNull( nameof( name ) );

         Name = name;
      }

      public string Name { get; }
      public abstract IPrivilege CreatePrivilege( );

      public override string ToString( ) {
         return Name.ToLower( );
      }

      public override bool Equals( object obj ) {
         if( obj == null )
            return false;

         if( !( obj is PrivilegeClassBase other ) )
            return false;

         return GetHashCode( ) == other.GetHashCode( );
      }

      public override int GetHashCode( ) {
         return Name.GetHashCode( );
      }

   }

}
