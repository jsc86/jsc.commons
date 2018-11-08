// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.behaving;
using jsc.commons.behaving.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.rc {

   public class RuleCheckerBase<T> : IRuleChecker<T> where T : class {

      private readonly List<IRule<T>> _rules = new List<IRule<T>>( );

      public RuleCheckerBase( ) { }

      public RuleCheckerBase( IEnumerable<IRule<T>> rules ) {
         _rules.AddRange( rules );
      }

      public IViolation<T> Check( T subject, IBehaviors context = null ) {
         context = context??EmptyBehaviors.Instance;
         foreach( IRule<T> rule in _rules ) {
            IViolation<T> violation = rule.Check( subject, context );
            if( violation != NonViolation<T>.Instance )
               return violation;
         }

         return NonViolation<T>.Instance;
      }

      public bool Check( T subject, out IEnumerable<IViolation<T>> violations, IBehaviors context = null ) {
         context = context??EmptyBehaviors.Instance;
         violations = new List<IViolation<T>>( );
         foreach( IRule<T> rule in _rules ) {
            IViolation<T> violation = rule.Check( subject, context );
            if( violation != NonViolation<T>.Instance )
               ( (List<IViolation<T>>)violations ).Add( violation );
         }

         return ( (List<IViolation<T>>)violations ).Count == 0;
      }

      public IEnumerable<IRule<T>> Rules => _rules;

      public void Add( IRule<T> rule ) {
         _rules.Add( rule );
      }

      public bool Remove( IRule<T> rule ) {
         return _rules.Remove( rule );
      }

   }

}