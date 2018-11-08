// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

namespace jsc.commons.rc.interfaces {

   /// <summary>
   ///    A Violation of a Rule - created by a Rule.
   ///    It has a list of possible Solutions for altering a subject to satisfy this Rule
   ///    (possibly empty).
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public interface IViolation<T> where T : class {

      /// <summary>
      ///    The Rule which created this Violation.
      /// </summary>
      IRule<T> Rule { get; }

      /// <summary>
      ///    A list of possible Solutions to alter the subject to satisfy the Rule
      ///    (possibly empty).
      /// </summary>
      IEnumerable<ISolution<T>> Solutions { get; }

      /// <summary>
      ///    A short human-readable description of this Violation.
      /// </summary>
      string Description { get; }

      bool HasSolution { get; }

   }

}