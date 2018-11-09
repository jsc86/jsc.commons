// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.behaving.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.rc.generic.rules {

   public class True<T> : RuleBase<T> where T : class {

      public override IViolation<T> Check( T subject, IBehaviors context = null ) {
         return NonViolation<T>.Instance;
      }

      public static True<T> Instance { get; } = new True<T>( );

   }

}
