// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;

using jsc.commons.config.interfaces;

namespace jsc.commons.config {

   public class DefaultsProviderBase : IDefaultsProvider {

      private readonly Dictionary<string, object> _defaults;

      protected DefaultsProviderBase( IEnumerable<Tuple<string, object>> defaults ) {
         IEnumerable<Tuple<string, object>> defaultsL = defaults as IList<Tuple<string, object>>??defaults.ToList( );
         _defaults = new Dictionary<string, object>( defaultsL.Count( ) );
         foreach( Tuple<string, object> d in defaultsL )
            _defaults[ d.Item1 ] = d.Item2;
      }

      public bool TryGetDefault( string propName, out object value ) {
         return _defaults.TryGetValue( propName, out value );
      }

      public object GetDefault( string propName, bool throwIfNull = false ) {
         object value;
         if( !TryGetDefault( propName, out value )&&throwIfNull )
            throw new ApplicationException( $"no default for '{propName}'" );

         return value;
      }

   }

}