// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Linq;
using System.Text;

using jsc.commons.naming.interfaces;

namespace jsc.commons.naming.styles {

   public class CamelCase : INamingStyle {

      public static readonly INamingStyle Instance = new CamelCase( );

      public string Name => "camelCase";

      public string ToString( UnifiedName unifiedName ) {
         StringBuilder sb = new StringBuilder( );
         sb.Append( unifiedName.Parts.First( ).ToLower( ) );
         foreach( string p in unifiedName.Parts ) {
            if( p == unifiedName.Parts.First( ) )
               continue;

            if( char.IsUpper( p[ 0 ] ) ) {
               sb.Append( p );
            } else {
               sb.Append( char.ToUpper( p[ 0 ] ) );
               sb.Append( p.Substring( 1 ) );
            }
         }

         return sb.ToString( );
      }

      public UnifiedName FromString( string name ) {
         if( char.IsUpper( name[ 0 ] ) )
            throw new ApplicationException( $"{nameof( name )} is not in {Name} notation" );

         return PascalCase.Parse( name );
      }

   }

}