// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.behaving.interfaces;

namespace jsc.commons.rc.interfaces {

   /// <summary>
   ///    A Rule defines a constraint on a subject and is able to
   ///    perform a check, whether the subject satisfies this constraint or not.
   ///    It (should) provides a means to alter a subject to either to
   ///    either fulfill or violate this constraint.
   ///    The ability to violate the constraint is required if a hierarchically
   ///    superior Rule negates another Rule to enable the superior Rule
   ///    to make the subject valid.
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public interface IRule<T> where T : class {

      /// <summary>
      ///    A short human-readable description of this Rule.
      /// </summary>
      string Description { get; }

      /// <summary>
      ///    Check if the given subject satisfies this Rule.
      ///    If the Rule is satisfied a NonViolation is returned.
      /// </summary>
      /// <param name="subject"></param>
      /// <param name="context"></param>
      /// <returns></returns>
      IViolation<T> Check( T subject, IBehaviors context );

      /// <summary>
      ///    Returns a list of Solutions which can make the subject satisfy this Rule.
      ///    (possibly none)
      /// </summary>
      /// <returns></returns>
      IEnumerable<ISolution<T>> MakeValid( );

      /// <summary>
      ///    Returns a list of Solutions which can make the subject violate this Rule.
      ///    (possibly none)
      /// </summary>
      /// <returns></returns>
      IEnumerable<ISolution<T>> MakeInvalid( );

   }

}