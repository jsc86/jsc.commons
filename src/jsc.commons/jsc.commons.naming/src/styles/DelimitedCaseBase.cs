// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Linq;

using jsc.commons.naming.interfaces;

namespace jsc.commons.naming.styles {

   public class DelimitedCaseBase : INamingStyle {

      private readonly char _delimiter;

      public DelimitedCaseBase( char delimiter ) {
         _delimiter = delimiter;
      }

      public virtual string Name => $"{_delimiter}delimited";

      public string ToString( UnifiedName unifiedName ) {
         return unifiedName.Parts.Aggregate(
               ( a, b ) => $"{FormatWordOrAbbreviation( a )}{_delimiter}{FormatWordOrAbbreviation( b )}" );
      }

      public UnifiedName FromString( string name ) {
         return new UnifiedName( Array.ConvertAll( name.Split( _delimiter ), ParseWordOrAbbreviation ) );
      }

      protected virtual string FormatWordOrAbbreviation( string part ) {
         return part.ToLower( );
      }

      protected virtual string ParseWordOrAbbreviation( string woa ) {
         return woa.ToLower( );
      }

   }

}