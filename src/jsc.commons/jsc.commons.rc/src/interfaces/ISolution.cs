// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

namespace jsc.commons.rc.interfaces {

   /// <summary>
   ///    A Solution to the Violation of a Rule
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public interface ISolution<T> where T : class {

      /// <summary>
      ///    A list of Actions to apply on a subject in order
      ///    to satisfy a Rule which created the Violation containing
      ///    this Solution.
      /// </summary>
      IEnumerable<IAction<T>> Actions { get; }

      /// <summary>
      ///    A short human-readable description of this Solution.
      /// </summary>
      string Description { get; }

   }

}