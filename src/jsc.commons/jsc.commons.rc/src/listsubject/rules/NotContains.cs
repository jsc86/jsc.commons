// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.rc.generic.rules;

namespace jsc.commons.rc.listsubject.rules {

   public class NotContains<T> : Not<IList<T>> where T : class {

      // todo: consider complete removal of this class

      private string _description;

//      private readonly Lazy<IEnumerable<ISolution<IList<T>>>> _lazyMakeValid;
//      private readonly Lazy<IEnumerable<ISolution<IList<T>>>> _lazyMakeInvalid;

      public NotContains( T target ) : base( new Not<IList<T>>( new Contains<T>( target ) ) ) {
         Target = target;
//         _lazyMakeValid = new Lazy<IEnumerable<ISolution<IList<T>>>>(
//            () => new ReadOnlyCollection<ISolution<IList<T>>>(
//               new ISolution<IList<T>>[]{
//                  new Solution<IList<T>>(
//                     new[]{
//                        new Remove<T>( Target )
//                     } )
//               } ) );
//         _lazyMakeInvalid = new Lazy<IEnumerable<ISolution<IList<T>>>>(
//            () => new ReadOnlyCollection<ISolution<IList<T>>>(
//               new ISolution<IList<T>>[]{
//                  new Solution<IList<T>>(
//                     new[]{
//                        new Add<T>( Target )
//                     } )
//               } ) );
      }

      public T Target { get; }

//      public override IViolation<IList<T>> Check( IList<T> subject ){
//         if( !subject.Contains( Target ) )
//            return NonViolation<IList<T>>.Instance;
//
//         return new Violation<IList<T>>(
//            this,
//            MakeValid() );
//      }
//

//      public override IEnumerable<ISolution<IList<T>>> MakeValid() => _lazyMakeValid.Instance;
//
//      public override IEnumerable<ISolution<IList<T>>> MakeInvalid() => _lazyMakeInvalid.Instance;

      public override string Description => _description??( _description = $"not contains {Target}" );

   }

}