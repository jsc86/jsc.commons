// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

using jsc.commons.behaving.interfaces;

namespace jsc.commons.behaving {

   public class BehaviorsBase : IBehaviors {

      private readonly misc.Lazy<Dictionary<Type, object>> _lazyBehaviors;

      public BehaviorsBase( ) {
         _lazyBehaviors = new misc.Lazy<Dictionary<Type, object>>(
               ( ) => new Dictionary<Type, object>( 1 ) );
      }


      public T Get<T>( ) where T : IBehavior {
         if( !_lazyBehaviors.IsInitialized )
            throw new ArgumentException( $"no behavior of type {typeof( T )} set" );

         object behavior;
         if( _lazyBehaviors.Instance.TryGetValue( typeof( T ), out behavior ) )
            return (T)behavior;

         throw new ArgumentException( $"no behavior of type {typeof( T )} set" );
      }

      public bool TryGet<T>( out T behavior ) where T : IBehavior {
         if( !_lazyBehaviors.IsInitialized ) {
            behavior = default( T );
            return false;
         }

         object behaviorObj;
         if( _lazyBehaviors.Instance.TryGetValue( typeof( T ), out behaviorObj ) ) {
            behavior = (T)behaviorObj;
            return true;
         }

         behavior = default( T );
         return false;
      }

      public void Set( IBehavior behavior ) {
         _lazyBehaviors.Instance[ behavior.GetType( ) ] = behavior;
      }

   }

}
