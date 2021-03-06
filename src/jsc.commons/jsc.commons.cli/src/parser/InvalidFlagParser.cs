// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.cli.config;
using jsc.commons.cli.interfaces;

namespace jsc.commons.cli.parser {

   public class InvalidFlagParser : IParser<object> {

      private readonly Func<char, char, bool> _cmpChars;

      private readonly string _flagPrefix;
      private int _index = -1;
      private bool _match = true;

      public InvalidFlagParser( ICliSpecification spec ) {
         ICliConfig conf = spec.Config;
         _flagPrefix = spec.Config.FlagPrefix( );
         _cmpChars = conf.CaseSensitiveFlags
               ? (Func<char, char, bool>)( ( c1, c2 ) => c1 == c2 )
               : ( c1, c2 ) => char.ToUpperInvariant( c1 ) == char.ToUpperInvariant( c2 );
      }

      public bool Next( char c ) {
         if( !_match )
            return false;

         _index++;
         if( _index >= _flagPrefix.Length )
            return _match;
         if( !_cmpChars( _flagPrefix[ _index ], c ) )
            return _match = false;

         return _match;
      }

      public object Done( ) {
         throw new NotImplementedException( );
      }

   }

}