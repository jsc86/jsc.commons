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

   public class Solution<T> : ISolution<T> where T : class {

      private string _description;

      public Solution( IEnumerable<IAction<T>> actions ) {
         Actions = actions as ReadOnlyCollection<IAction<T>>??
               new ReadOnlyCollection<IAction<T>>(
                     actions as IList<IAction<T>>??actions.ToList( ) );
      }

      public IEnumerable<IAction<T>> Actions { get; }

      public string Description =>
            _description ??= string.Join( ", ", Actions.Select( a => a.Description ) );

   }

}