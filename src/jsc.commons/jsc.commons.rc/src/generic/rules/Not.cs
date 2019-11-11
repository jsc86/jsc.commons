// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.behaving.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.rc.generic.rules {

   public class Not<T> : RuleBase<T> where T : class {

      private string _description;

      public Not( IRule<T> target ) {
         Target = target;
      }

      public IRule<T> Target { get; }

      public override string Description => _description??( _description = $"not {Target.Description}" );

      public override IViolation<T> Check( T subject, IBehaviors context = null ) {
         IViolation<T> violation = Target.Check( subject, context );

         return violation == NonViolation<T>.Instance
               ? (IViolation<T>)new Violation<T>(
                     this,
                     Reduce(
                           MakeInvalid( ),
                           subject ) )
               : NonViolation<T>.Instance;
      }

      public override IEnumerable<ISolution<T>> MakeInvalid( ) {
         return Target.MakeValid( );
      }

      public override IEnumerable<ISolution<T>> MakeValid( ) {
         return Target.MakeInvalid( );
      }

   }

}
