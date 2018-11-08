// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using jsc.commons.behaving.interfaces;
using jsc.commons.misc;
using jsc.commons.rc.interfaces;

namespace jsc.commons.rc.generic.rules {

   public class Or<T> : RuleBase<T> where T : class {

      private readonly Lazy<IEnumerable<ISolution<T>>> _lazyMakeInvalid;

      private readonly Lazy<IEnumerable<ISolution<T>>> _lazyMakeValid;

      private string _description;

      public Or( IRule<T> targetA, IRule<T> targetB ) {
         TargetA = targetA;
         TargetB = targetB;
         _lazyMakeValid = new Lazy<IEnumerable<ISolution<T>>>(
               ( ) => new ReadOnlyCollection<ISolution<T>>(
                     TargetA.MakeValid( ).Union( TargetB.MakeValid( ) ).ToList( ) ) );
         _lazyMakeInvalid = new Lazy<IEnumerable<ISolution<T>>>(
               ( ) => new ReadOnlyCollection<ISolution<T>>(
                     TargetA.MakeInvalid( )
                           .Join(
                                 TargetB.MakeInvalid( ),
                                 sa => true,
                                 sb => true,
                                 ( sa, sb ) => (ISolution<T>)new Solution<T>( sa.Actions.Union( sb.Actions ) ) )
                           .ToList( )
               ) );
      }

      public IRule<T> TargetA { get; }

      public IRule<T> TargetB { get; }

      public override string Description =>
            _description??( _description = $"({TargetA.Description} or {TargetB.Description})" );

      public override IViolation<T> Check( T subject, IBehaviors context = null ) {
         IViolation<T> va = TargetA.Check( subject, context );
         if( va == NonViolation<T>.Instance )
            return va;

         IViolation<T> vb = TargetB.Check( subject, context );
         if( vb == NonViolation<T>.Instance )
            return vb;

         return new Violation<T>(
               this,
               MakeValid( ) );
      }

      public override IEnumerable<ISolution<T>> MakeValid( ) {
         return _lazyMakeValid.Instance;
      }

      public override IEnumerable<ISolution<T>> MakeInvalid( ) {
         return _lazyMakeInvalid.Instance;
      }

   }

}