// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Linq;

using jsc.commons.behaving.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.rc.generic.rules {

   public class False<T> : RuleBase<T> where T : class {

      public static False<T> Instance { get; } = new False<T>( );

      public override IViolation<T> Check( T subject, IBehaviors context = null ) {
         return new Violation<T>(
               this,
               Enumerable.Empty<ISolution<T>>( ) );
      }

   }

}