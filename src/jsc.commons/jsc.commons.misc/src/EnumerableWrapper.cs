// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections;
using System.Collections.Generic;

namespace jsc.commons.misc {

   public class EnumerableWrapper<T> : IEnumerable<T> {

      private readonly IEnumerable<T> _client;

      public EnumerableWrapper( IEnumerable<T> client ) {
         _client = client;
      }

      public IEnumerator<T> GetEnumerator( ) {
         return _client.GetEnumerator( );
      }

      IEnumerator IEnumerable.GetEnumerator( ) {
         return GetEnumerator( );
      }

   }

}