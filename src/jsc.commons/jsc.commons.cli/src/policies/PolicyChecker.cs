// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.behaving;
using jsc.commons.behaving.interfaces;
using jsc.commons.cli.config;
using jsc.commons.cli.interfaces;
using jsc.commons.rc;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.policies {

   public class PolicyChecker {

      private readonly ICliConfig _config;

      public PolicyChecker( ICliConfig config ) {
         _config = config;
      }


      public void Check( ICliSpecification spec ) {
         IBehaviors context = new BehaviorsBase( );
         context.Set( new ConfigBehavior( _config ) );
         foreach( IRule<ICliSpecification> policy in _config.Policies ) {
            IViolation<ICliSpecification> violation = policy.Check( spec, context );
            if( violation != NonViolation<ICliSpecification>.Instance )
               throw new CliPolicyException( policy, violation );
         }
      }

   }

}