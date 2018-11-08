// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Linq;

using jsc.commons.behaving.interfaces;
using jsc.commons.cli.interfaces;
using jsc.commons.rc;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.policies {

   public class SingleItemArguments : RuleBase<ICliSpecification> {

      public override IViolation<ICliSpecification> Check(
            ICliSpecification subject,
            IBehaviors context = null ) {
         IItem item = subject.Options.Cast<IItem>( )
               .Union( subject.Flags )
               .FirstOrDefault( i => i.Arguments.Count( ) > 1 );
         if( item != null )
            return new Violation<ICliSpecification>(
                  this,
                  Enumerable.Empty<ISolution<ICliSpecification>>( ),
                  $"{( item is IOption? "option" : "flag" )} has more than one argument" );

         return NonViolation<ICliSpecification>.Instance;
      }

   }

}