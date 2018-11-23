﻿// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;

namespace jsc.commons.naming {

   public class UnifiedName : IComparable<UnifiedName> {

      private readonly string[] _parts;

      public UnifiedName( IEnumerable<string> parts ) : this( parts.ToArray( ) ) { }

      public UnifiedName( params string[] parts ) {
         _parts = parts;
      }

      public IEnumerable<string> Parts => _parts;

      public override string ToString( ) {
         return string.Join( ".", _parts );
      }

      public override bool Equals( object obj ) {
         UnifiedName other = obj as UnifiedName;
         if( _parts.Length != other?._parts.Length )
            return false;

         for( int i = 0,
               l = _parts.Length;
               i < l;
               i++ )
            if( _parts[ i ] != other._parts[ i ] )
               return false;

         return true;
      }

      public int CompareTo( UnifiedName other ) {
         int i = 0;
         while( true ) {
            if( _parts.Length == i
                  &&other._parts.Length == i )
               return 0;
            if( _parts.Length == i )
               return -1;
            if( other._parts.Length == i )
               return 1;

            string part = _parts[ i ];
            string otherPart = other._parts[ i ];
            int r = string.Compare( part, otherPart, StringComparison.Ordinal );

            if( r != 0 )
               return r;

            i++;
         }
      }

   }

}
