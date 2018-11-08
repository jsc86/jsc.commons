// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.behaving.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.rc.listsubject.actions {

   public class Remove<T> : IAction<IList<T>> {

      private string _description;

      public Remove( T target ) {
         Target = target;
      }

      public T Target { get; }

      public void Apply( IList<T> subject, IBehaviors context ) {
         subject.Remove( Target );
      }

      public bool Contradicts( IAction<IList<T>> a ) {
         return a is Add<T> add&&Target.Equals( add.Target );
      }

      public string Description => _description??( _description = $"remove {Target}" );

      public bool IsInteractive { get; } = false;

   }

}