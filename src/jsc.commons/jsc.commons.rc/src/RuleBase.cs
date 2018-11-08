// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Linq;

using jsc.commons.behaving.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.rc {

   public abstract class RuleBase<T> : IRule<T> where T : class {

      public abstract IViolation<T> Check( T subject, IBehaviors context = null );

      public virtual string Description => GetType( ).Name;

      public virtual IEnumerable<ISolution<T>> MakeValid( ) {
         return Enumerable.Empty<ISolution<T>>( );
      }

      public virtual IEnumerable<ISolution<T>> MakeInvalid( ) {
         return Enumerable.Empty<ISolution<T>>( );
      }

   }

}