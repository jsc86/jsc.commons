// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.behaving.interfaces;
using jsc.commons.misc;

namespace jsc.commons.behaving {

   public class BehaviorsContainerBase : IBehaviorsContainer {

      public BehaviorsContainerBase( ) {
         LazyBehaviors = new Lazy<IBehaviors>(
               ( ) => new BehaviorsBase( ) );
      }

      internal Lazy<IBehaviors> LazyBehaviors { get; }

      public IBehaviors Behaviors => LazyBehaviors.Instance;

   }

}