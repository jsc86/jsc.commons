// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Linq;

using jsc.commons.rc.interfaces;

namespace jsc.commons.rc {

   public class NonViolation<T> : IViolation<T> where T : class {

      public static NonViolation<T> Instance { get; } = new NonViolation<T>( );

      public IRule<T> Rule => null;

      public IEnumerable<ISolution<T>> Solutions => Enumerable.Empty<ISolution<T>>( );

      public string Description => string.Empty;

      public bool HasSolution => false;

   }

}