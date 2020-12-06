// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Text;

using jsc.commons.naming.interfaces;

namespace jsc.commons.naming.styles {

   public class PascalCase : INamingStyle {

      public static readonly INamingStyle Instance = new PascalCase( );

      public string Name => "PascalCase";

      public string ToString( UnifiedName unifiedName ) {
         StringBuilder sb = new StringBuilder( );
         foreach( string p in unifiedName.Parts )
            if( char.IsUpper( p[ 0 ] ) ) {
               sb.Append( p );
            } else {
               sb.Append( char.ToUpper( p[ 0 ] ) );
               sb.Append( p.Substring( 1 ) );
            }

         return sb.ToString( );
      }

      public UnifiedName FromString( string name ) {
         if( char.IsLower( name[ 0 ] ) )
            throw new ApplicationException( $"{nameof( name )} is not in {Name} notation" );

         return Parse( name );
      }

      internal static UnifiedName Parse( string name ) {
         List<string> parts = new List<string>( );
         StringBuilder sb = new StringBuilder( );
         char cPrev = ' ';
         bool abbrev = false;
         foreach( char c in name ) {
            if( char.IsUpper( c )
                  &&char.IsLower( cPrev ) ) {
               // complete word
               parts.Add( sb.ToString( ).ToLower( ) );
               sb.Clear( );
            }

            if( abbrev&&char.IsLower( c ) ) {
               // complete abbreviation
               parts.Add( sb.ToString( ).Substring( 0, sb.Length-1 ) );
               sb.Remove( 0, sb.Length-1 );
            }

            abbrev = char.IsUpper( c )&&char.IsUpper( cPrev );
            sb.Append( c );
            cPrev = c;
         }

         parts.Add( char.IsUpper( sb[ ^1 ] )? sb.ToString( ) : sb.ToString( ).ToLower( ) );

         return new UnifiedName( parts );
      }

   }

}