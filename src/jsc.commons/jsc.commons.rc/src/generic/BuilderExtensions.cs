// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.rc.generic.rules;
using jsc.commons.rc.interfaces;

namespace jsc.commons.rc.generic {

   public static class BuilderExtensions {

      public static And<T> And<T>( this IRule<T> @this, IRule<T> other ) where T : class {
         return new And<T>( @this, other );
      }

      public static Or<T> Or<T>( this IRule<T> @this, IRule<T> other ) where T : class {
         return new Or<T>( @this, other );
      }

      public static Implies<T> Implies<T>( this IRule<T> @this, IRule<T> other ) where T : class {
         return new Implies<T>( @this, other );
      }

      public static Xor<T> Xor<T>( this IRule<T> @this, IRule<T> other ) where T : class {
         return new Xor<T>( @this, other );
      }

   }

}
