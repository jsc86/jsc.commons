// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

namespace jsc.commons.behaving.interfaces {

   public interface IBehaviors {

      IEnumerable<object> Objects { get; }

      T Get<T>( ) where T : IBehavior;

      bool TryGet<T>( out T behavior ) where T : IBehavior;

      void Set( IBehavior behavior );

   }

}