// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using jsc.commons.rc.interfaces;

namespace jsc.commons.rc {

   public class Violation<T> : IViolation<T> where T : class {

      private string _description;

      public Violation(
            IRule<T> rule,
            IEnumerable<ISolution<T>> solutions,
            string description = null ) {
         Rule = rule;
         Solutions = solutions as ReadOnlyCollection<ISolution<T>>??
               new ReadOnlyCollection<ISolution<T>>( solutions as IList<ISolution<T>>??solutions.ToList( ) );
         _description = description;
         HasSolution = ( (ReadOnlyCollection<ISolution<T>>)Solutions ).Count > 0;
      }

      public IRule<T> Rule { get; }

      public IEnumerable<ISolution<T>> Solutions { get; }

      public string Description => _description ??= $"violated: {Rule.Description}";

      public bool HasSolution { get; }

   }

}