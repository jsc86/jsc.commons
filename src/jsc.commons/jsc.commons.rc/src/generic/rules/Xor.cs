// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Linq;

using jsc.commons.behaving.interfaces;
using jsc.commons.misc;
using jsc.commons.rc.interfaces;

namespace jsc.commons.rc.generic.rules {

   public class Xor<T> : RuleBase<T> where T : class {

      private readonly Lazy<IEnumerable<ISolution<T>>> _lazyMakeInvalid;

      private readonly Lazy<IEnumerable<ISolution<T>>> _lazyMakeValid;

      private readonly IRule<T>[] _targets;

      private string _description;

      public Xor( IRule<T> target1, IRule<T> target2, params IRule<T>[] targets ) {
         _targets = new[] {target1, target2}.Union( targets ).ToArray( );
         _lazyMakeValid = new Lazy<IEnumerable<ISolution<T>>>( GetValidSolutions );
         _lazyMakeInvalid = new Lazy<IEnumerable<ISolution<T>>>( GetInvalidSolutions );
      }

      private IEnumerable<IRule<T>> Targets => new EnumerableWrapper<IRule<T>>( _targets );

      public override string Description =>
            _description??( _description =
                  $"xor({_targets.Select( t => t.Description ).Aggregate( ( a, b ) => $"{a}, {b}" )})" );

      private IEnumerable<ISolution<T>> GetInvalidSolutions( ) {
         // todo: add more solutions
         return new ISolution<T>[] {
               new Solution<T>(
                     _targets.SelectMany( t => t.MakeValid( ).SelectMany( s => s.Actions ) ) )
         };
      }

      private IEnumerable<ISolution<T>> GetValidSolutions( ) {
         List<Solution<T>> solutions = new List<Solution<T>>( _targets.Length );
         foreach( IRule<T> validTarget in _targets ) {
            IEnumerable<IRule<T>> invalidTargets = _targets.Except( new[] {validTarget} );
            solutions.Add(
                  new Solution<T>(
                        validTarget.MakeValid( )
                              .Union( invalidTargets.SelectMany( it => it.MakeInvalid( ) ) )
                              .SelectMany(
                                    t => t.Actions )
                  ) );
         }

         return solutions;
      }

      public override IViolation<T> Check( T subject, IBehaviors context = null ) {
         int nonViolationCount = 0;
         foreach( IRule<T> target in _targets ) {
            IViolation<T> violation = target.Check( subject, context );
            if( violation == NonViolation<T>.Instance )
               nonViolationCount++;
         }

         if( nonViolationCount == 1 )
            return NonViolation<T>.Instance;

         return new Violation<T>( this, MakeValid( ) );
      }

      public override IEnumerable<ISolution<T>> MakeValid( ) {
         return _lazyMakeValid.Instance;
      }

      public override IEnumerable<ISolution<T>> MakeInvalid( ) {
         return _lazyMakeInvalid.Instance;
      }

   }

}
