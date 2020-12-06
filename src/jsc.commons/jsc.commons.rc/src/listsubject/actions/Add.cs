// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.behaving.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.rc.listsubject.actions {

   public class Add<T> : IAction<IList<T>> {

      private string _description;

      public Add( T target ) {
         Target = target;
      }

      public T Target { get; }

      public void Apply( IList<T> subject, IBehaviors context ) {
         subject.Add( Target );
      }

      public bool Contradicts( IAction<IList<T>> a ) {
         return a is Remove<T> remove&&Target.Equals( remove.Target );
      }

      public bool ChangesSubject( IList<T> subject ) {
         return !subject.Contains( Target );
      }

      public string Description => _description ??= $"add {Target}";

      public bool IsInteractive { get; } = false;

   }

}