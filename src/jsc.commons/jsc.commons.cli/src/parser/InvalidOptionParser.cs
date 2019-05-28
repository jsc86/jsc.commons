// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.cli.interfaces;

namespace jsc.commons.cli.parser {

   public class InvalidOptionParser : IParser<object> {

      private readonly Func<char, char, bool> _cmpChars;

      private readonly string _optionPrefix;

      private int _index = -1;
      private bool _match = true;

      public InvalidOptionParser( ICliSpecification spec ) {
         _optionPrefix = spec.Config.OptionPrefix( );
         _cmpChars = spec.Config.CaseSensitiveOptions
               ? (Func<char, char, bool>)( ( c1, c2 ) => c1 == c2 )
               : ( c1, c2 ) => char.ToUpperInvariant( c1 ) == char.ToUpperInvariant( c2 );
      }

      public bool Next( char c ) {
         if( !_match )
            return false;

         _index++;
         if( _index >= _optionPrefix.Length )
            return true;
         if( !_cmpChars( _optionPrefix[ _index ], c ) )
            return _match = false;

         return _match;
      }

      public object Done( ) {
         throw new NotImplementedException( );
      }

   }

}