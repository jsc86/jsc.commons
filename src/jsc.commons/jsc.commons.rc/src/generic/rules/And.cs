// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using jsc.commons.behaving.interfaces;
using jsc.commons.misc;
using jsc.commons.rc.interfaces;

namespace jsc.commons.rc.generic.rules {

   public class And<T> : RuleBase<T> where T : class {

      private readonly Lazy<IEnumerable<ISolution<T>>> _lazyMakeInvalid;

      private readonly Lazy<IEnumerable<ISolution<T>>> _lazyMakeValid;

      private string _description;

      public And( IRule<T> targetA, IRule<T> targetB ) {
         TargetA = targetA;
         TargetB = targetB;
         _lazyMakeValid = new Lazy<IEnumerable<ISolution<T>>>(
               ( ) => new ReadOnlyCollection<ISolution<T>>(
                     TargetA.MakeValid( )
                           .Join(
                                 TargetB.MakeValid( ),
                                 sa => true,
                                 sb => true,
                                 ( sa, sb ) => (ISolution<T>)new Solution<T>( sa.Actions.Union( sb.Actions ) ) )
                           .ToList( )
               ) );
         _lazyMakeInvalid = new Lazy<IEnumerable<ISolution<T>>>(
               ( ) => new ReadOnlyCollection<ISolution<T>>(
                     TargetA.MakeInvalid( ).Union( TargetB.MakeInvalid( ) ).ToList( ) ) );
      }

      public And( IRule<T> targetA, IRule<T> targetB, params IRule<T>[] targets ) :
            this( targetA, Chain( new[] {targetB}.Union( targets ).ToArray( ) ) ) { }

      public IRule<T> TargetA { get; }

      public IRule<T> TargetB { get; }

      public override string Description =>
            _description ??= $"({TargetA.Description} and {TargetB.Description})";

      private static IRule<T> Chain( IRule<T>[] targets ) {
         int l = targets.Length;
         And<T> current = new And<T>( targets[ l-2 ], targets[ l-1 ] );
         for( int i = l-3; i >= 0; i-- )
            current = new And<T>( targets[ i ], current );

         return current;
      }

      public override IViolation<T> Check( T subject, IBehaviors context = null ) {
         IViolation<T> va = TargetA.Check( subject, context );
         IViolation<T> vb = TargetB.Check( subject, context );

         if( va == NonViolation<T>.Instance
               &&vb == NonViolation<T>.Instance )
            return NonViolation<T>.Instance;

         return new Violation<T>(
               this,
               Reduce(
                     MakeValid( ),
                     subject ) );
      }

      public override IEnumerable<ISolution<T>> MakeValid( ) {
         return _lazyMakeValid.Instance;
      }

      public override IEnumerable<ISolution<T>> MakeInvalid( ) {
         return _lazyMakeInvalid.Instance;
      }

   }

}