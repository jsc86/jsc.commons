// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.behaving;
using jsc.commons.behaving.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.rc.generic {

   public class GenericAction<T> : IAction<T> where T : class {

      private readonly Action<T, IBehaviors> _apply;
      private readonly Func<T, bool> _changesSubject;
      private readonly Func<IAction<T>, bool> _contradicts;

      public GenericAction(
            string description,
            Action<T, IBehaviors> apply = null,
            Func<IAction<T>, bool> contradicts = null,
            bool isInteractive = false,
            Func<T, bool> changesSubject = null ) {
         Description = description;
         _apply = apply;
         _contradicts = contradicts;
         IsInteractive = isInteractive;
         _changesSubject = changesSubject;
      }

      public void Apply( T subject, IBehaviors context ) {
         _apply?.Invoke( subject, context??EmptyBehaviors.Instance );
      }

      public bool Contradicts( IAction<T> a ) {
         return _contradicts != null&&_contradicts( a );
      }

      public bool ChangesSubject( T subject ) {
         return _changesSubject == null||_changesSubject( subject );
      }

      public string Description { get; }

      public bool IsInteractive { get; }

   }

}