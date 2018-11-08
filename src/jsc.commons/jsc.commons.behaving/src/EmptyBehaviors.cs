// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.behaving.interfaces;

namespace jsc.commons.behaving {

   public class EmptyBehaviors : IBehaviors {

      private EmptyBehaviors( ) { }

      public static IBehaviors Instance { get; } = new EmptyBehaviors( );

      public T Get<T>( ) where T : IBehavior {
         throw new ArgumentException( $"no behavior of type {typeof( T )} set" );
      }

      public bool TryGet<T>( out T behavior ) where T : IBehavior {
         behavior = default( T );
         return false;
      }

      public void Set( IBehavior behavior ) {
         throw new NotImplementedException( );
      }

   }

}