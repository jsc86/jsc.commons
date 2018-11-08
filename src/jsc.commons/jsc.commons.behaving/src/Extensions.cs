// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.behaving.interfaces;

namespace jsc.commons.behaving {

   public static class Extensions {

      public static T Get<T>( this IBehaviorsContainer bc ) where T : IBehavior {
         if( bc is BehaviorsContainerBase bcb
               &&!bcb.LazyBehaviors.IsInitialized )
            throw new ArgumentException( $"no behavior of type {typeof( T )} set" );

         return bc.Behaviors.Get<T>( );
      }

      public static bool TryGet<T>( this IBehaviorsContainer bc, out T behavior ) where T : IBehavior {
         if( bc is BehaviorsContainerBase bcb
               &&!bcb.LazyBehaviors.IsInitialized ) {
            behavior = default( T );
            return false;
         }

         return bc.Behaviors.TryGet( out behavior );
      }

      public static void Set( this IBehaviorsContainer bc, IBehavior behavior ) {
         bc.Behaviors.Set( behavior );
      }

   }

}