// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.behaving.interfaces;

namespace jsc.commons.rc.interfaces {

   /// <summary>
   ///    A Rule Checker holds a list of Rules and can perform
   ///    all corresponding Rule checks on a given subject in two different ways.
   ///    It can check the subject until the first Violation occurs and returns
   ///    a NonViolation if no Violation occurred or it can perform all checks at
   ///    once and return a boolean indicating whether Violations occurred.
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public interface IRuleChecker<T> where T : class {

      IEnumerable<IRule<T>> Rules { get; }

      /// <summary>
      ///    Checks for and returns the first Violation or NonViolation if no
      ///    Violation occurred for the given subject.
      /// </summary>
      /// <param name="subject"></param>
      /// <param name="context"></param>
      /// <returns></returns>
      IViolation<T> Check( T subject, IBehaviors context = null );

      /// <summary>
      ///    Checks for all Violations and returns a list of them.
      /// </summary>
      /// <param name="subject"></param>
      /// <param name="violations"></param>
      /// <returns>true, if no Violations occurred, otherwise false</returns>
      bool Check( T subject, out IEnumerable<IViolation<T>> violations, IBehaviors context = null );

      void Add( IRule<T> rule );

      bool Remove( IRule<T> rule );

   }

}