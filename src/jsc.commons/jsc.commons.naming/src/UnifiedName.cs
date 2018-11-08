// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Linq;

namespace jsc.commons.naming {

   public class UnifiedName {

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

   }

}