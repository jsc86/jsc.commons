// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

namespace jsc.commons.rc.tests {

   public class CList<T> : List<T>, ICloneable {

      public CList( ) { }

      public CList( IEnumerable<T> collection ) : base( collection ) { }

      public CList( int capacity ) : base( capacity ) { }

      public object Clone( ) {
         return new CList<T>( this );
      }

   }

}