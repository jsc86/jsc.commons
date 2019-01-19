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

   public class FlagParser : IParser<IEnumerable<IItem>> {

      private readonly Func<char, char, bool> _cmpChars;

      private readonly string _flagPrefix;
      private readonly List<IFlag> _flags;
      private readonly List<IItem> _matches;
      private readonly List<IOption> _options;
      private int _index = -1;
      private bool _match = true;

      public FlagParser( ICliSpecification spec ) {
         ICliConfig conf = spec.Config;
         _flags = spec.Flags.ToList( );
         _options = spec.Options.Where( o => o.FlagAlias.HasValue ).ToList( );
         _matches = new List<IItem>( );
         _flagPrefix = spec.Config.FlagPrefix( );
         _cmpChars = conf.CaseSensitiveFlags
               ? (Func<char, char, bool>)( ( c1, c2 ) => c1 == c2 )
               : ( c1, c2 ) => char.ToUpperInvariant( c1 ) == char.ToUpperInvariant( c2 );
      }

      public bool Next( char c ) {
         if( !_match )
            return false;

         _index++;
         if( _index < _flagPrefix.Length ) {
            if( !_cmpChars( _flagPrefix[ _index ], c ) )
               return _match = false;
         } else {
            IOption opt = _options.FirstOrDefault( o => o.FlagAlias.HasValue&&_cmpChars( o.FlagAlias.Value, c ) );
            IFlag flag = null;
            if( opt != null ) {
               _options.Remove( opt );
               _matches.Add( opt );
            } else {
               flag = _flags.FirstOrDefault( f => _cmpChars( f.Name, c ) );
               if( flag != null ) {
                  _flags.Remove( flag );
                  _matches.Add( flag );
               }
            }

            if( opt == null
                  &&flag == null )
               return _match = false;
         }

         return _match;
      }

      public IEnumerable<IItem> Done( ) {
         return _match? _matches : Enumerable.Empty<IItem>( );
      }

   }

}