// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;

using jsc.commons.cli.config;
using jsc.commons.cli.interfaces;

namespace jsc.commons.cli.parser {

   public class OptionParser : IParser<IOption> {

      private readonly Func<char, char, bool> _cmpChars;

      private readonly ICliConfig _conf;
      private readonly string _optionPrefix;
      private readonly List<IOption> _options;

      private int _index = -1;
      private bool _match = true;

      public OptionParser( ICliSpecification spec ) {
         _options = spec.Options.ToList( );
         _conf = spec.Config;
         _optionPrefix = spec.Config.OptionPrefix( );
         _cmpChars = _conf.CaseSensitiveOptions
               ? (Func<char, char, bool>)( ( c1, c2 ) => c1 == c2 )
               : ( c1, c2 ) => char.ToUpperInvariant( c1 ) == char.ToUpperInvariant( c2 );
      }

      public bool Next( char c ) {
         if( !_match )
            return false;

         _index++;
         if( _index < _optionPrefix.Length ) {
            if( !_cmpChars( _optionPrefix[ _index ], c ) )
               return _match = false;
         } else {
            IList<IOption> rem = null;
            int i = _index-_optionPrefix.Length;
            foreach( IOption option in _options ) {
               string deUnifiedName = option.GetDeUnifiedName( _conf );
               if( ( i >= deUnifiedName.Length )|!_cmpChars( deUnifiedName[ i ], c ) )
                  ( rem = rem??new List<IOption>( ) ).Add( option );
            }

            foreach( IOption option in rem??Enumerable.Empty<IOption>( ) )
               _options.Remove( option );

            if( _options.Count == 0 )
               return _match = false;
         }

         return _match;
      }

      public IOption Done( ) {
         return _match
               ? _options.FirstOrDefault(
                     o => o.GetDeUnifiedName( _conf ).Length == _index+1-_optionPrefix.Length )
               : null;
      }

   }

}