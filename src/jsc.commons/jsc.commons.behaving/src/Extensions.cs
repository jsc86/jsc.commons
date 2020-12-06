// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

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
            behavior = default;
            return false;
         }

         return bc.Behaviors.TryGet( out behavior );
      }

      public static void Set( this IBehaviorsContainer bc, IBehavior behavior ) {
         bc.Behaviors.Set( behavior );
      }

      public static T Get<T>( this object o ) where T : IBehavior {
         switch( o ) {
            case IBehaviors behaviors:
               return behaviors.Get<T>( );
            case IBehaviorsContainer container:
               return container.Get<T>( );
            default:
               throw new ArgumentException(
                     $"object {nameof( o )} of type {o.GetType( )} does not know how to behave" );
         }
      }

      public static bool TryGet<T>( this object o, out T behavior ) where T : IBehavior {
         switch( o ) {
            case IBehaviors behaviors:
               return behaviors.TryGet( out behavior );
            case IBehaviorsContainer container:
               return container.TryGet( out behavior );
            default:
               throw new ArgumentException(
                     $"object {nameof( o )} of type {o.GetType( )} does not know how to behave" );
         }
      }

      public static IEnumerable<object> Objects( this IBehaviorsContainer bc ) {
         return bc.Behaviors.Objects;
      }

      public static IEnumerable<object> Objects( this object o ) {
         switch( o ) {
            case IBehaviors behaviors:
               return behaviors.Objects;
            case IBehaviorsContainer container:
               return container.Objects( );
            default:
               throw new ArgumentException(
                     $"object {nameof( o )} of type {o.GetType( )} does not know how to behave" );
         }
      }

   }

}