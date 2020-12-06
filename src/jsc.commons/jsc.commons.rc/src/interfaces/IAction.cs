// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.behaving.interfaces;

namespace jsc.commons.rc.interfaces {

   /// <summary>
   ///    An Action represents a change to a subject.
   ///    If, for example, subject was a list, corresponding actions would
   ///    be "add" and "remove". Preferably each implementation of an
   ///    Action should have a corresponding counter Action, neutralizing
   ///    its effect on the subject.
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public interface IAction<T> where T : class {

      /// <summary>
      ///    A human readable short description of what this Action does.
      /// </summary>
      string Description { get; }

      /// <summary>
      ///    Indicates whether application of this action requires user interaction.
      /// </summary>
      bool IsInteractive { get; }

      /// <summary>
      ///    Applies this Action on the given subject.
      /// </summary>
      /// <param name="subject"></param>
      /// <param name="context"></param>
      void Apply( T subject, IBehaviors context );

      /// <summary>
      ///    Indicates whether this Action contradicts the
      ///    given Action "a" - i.e. this Action would alter
      ///    the change "a" has applied on a subject, if performed
      ///    on the same subject.
      /// </summary>
      /// <param name="a"></param>
      /// <returns></returns>
      bool Contradicts( IAction<T> a );

      /// <summary>
      ///    Indicates whether this action would actually change
      ///    the subject, if applied to it.
      /// </summary>
      /// <param name="subject"></param>
      /// <returns></returns>
      bool ChangesSubject( T subject );

   }

}