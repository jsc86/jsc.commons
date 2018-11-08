// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.naming.interfaces;

namespace jsc.commons.naming.styles {

   public class SnakeCaseUpper : DelimitedCaseBase {

      public static readonly INamingStyle Instance = new SnakeCaseUpper( );

      public SnakeCaseUpper( ) : base( '_' ) { }

      public override string Name => "SNAKE_CASE";

      protected override string FormatWordOrAbbreviation( string part ) {
         return part.ToUpper( );
      }

      protected override string ParseWordOrAbbreviation( string woa ) {
         return woa.ToLower( );
      }

   }

}