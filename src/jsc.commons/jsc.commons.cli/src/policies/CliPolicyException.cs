// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.cli.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.policies {

   public class CliPolicyException : Exception {

      public CliPolicyException( IRule<ICliSpecification> policy, IViolation<ICliSpecification> violation )
            : base( $"{policy.Description}: {violation.Description}" ) {
         Policy = policy;
         Violation = violation;
      }

      public IViolation<ICliSpecification> Violation { get; }

      public IRule<ICliSpecification> Policy { get; }

   }

}