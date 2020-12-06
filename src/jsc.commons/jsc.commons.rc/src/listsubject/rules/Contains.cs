// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Collections.ObjectModel;

using jsc.commons.behaving.interfaces;
using jsc.commons.misc;
using jsc.commons.rc.interfaces;
using jsc.commons.rc.listsubject.actions;

namespace jsc.commons.rc.listsubject.rules {

   public class Contains<T> : RuleBase<IList<T>> where T : class {

      private readonly Lazy<IEnumerable<ISolution<IList<T>>>> _lazyMakeInvalid;
      private readonly Lazy<IEnumerable<ISolution<IList<T>>>> _lazyMakeValid;

      private string _description;

      public Contains( T target ) {
         Target = target;
         _lazyMakeValid = new Lazy<IEnumerable<ISolution<IList<T>>>>(
               ( ) => new ReadOnlyCollection<ISolution<IList<T>>>(
                     new ISolution<IList<T>>[] {
                           new Solution<IList<T>>(
                                 new[] {
                                       new Add<T>( Target )
                                 } )
                     } ) );
         _lazyMakeInvalid = new Lazy<IEnumerable<ISolution<IList<T>>>>(
               ( ) => new ReadOnlyCollection<ISolution<IList<T>>>(
                     new ISolution<IList<T>>[] {
                           new Solution<IList<T>>(
                                 new[] {
                                       new Remove<T>( Target )
                                 } )
                     } ) );
      }

      public T Target { get; }

      public override string Description => _description ??= $"contains {Target}";

      public override IViolation<IList<T>> Check( IList<T> subject, IBehaviors context ) {
         if( subject.Contains( Target ) )
            return NonViolation<IList<T>>.Instance;

         return new Violation<IList<T>>(
               this,
               MakeValid( ) );
      }

      public override IEnumerable<ISolution<IList<T>>> MakeValid( ) {
         return _lazyMakeValid.Instance;
      }

      public override IEnumerable<ISolution<IList<T>>> MakeInvalid( ) {
         return _lazyMakeInvalid.Instance;
      }

   }

}