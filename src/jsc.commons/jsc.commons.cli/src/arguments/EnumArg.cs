// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

namespace jsc.commons.cli.arguments {

   public class EnumArg<T> : Argument<T> where T : struct, IConvertible {

      public EnumArg( string name, string description, bool optional = true ) : base( name, description, optional ) {
         if( !typeof( T ).IsEnum )
            throw new ArgumentException( $"{nameof( T )} must be an enum" );
      }

      protected override object ParseInternal( string value ) {
         if( value == null )
            return null;

         return Enum.Parse( typeof( T ), value, true );
      }

   }

}