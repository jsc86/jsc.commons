// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

namespace jsc.commons.cli.arguments {

   public class BoolArg : Argument<bool> {

      public BoolArg( string name, bool optional = true ) : base( name, optional ) { }

      protected override object ParseInternal( string value ) {
         if( value == null )
            return null;

         return bool.Parse( value );
      }

   }

}