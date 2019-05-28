// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.behaving.interfaces;
using jsc.commons.cli.interfaces;
using jsc.commons.rc;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.help {

   internal class HelpIsSetOr : RuleBase<IParserResult> {

      private readonly HelpOption _target;

      public HelpIsSetOr( HelpOption target, IRule<IParserResult> other ) {
         _target = target;
         Other = other;
      }

      public IItem Target => _target;

      public IRule<IParserResult> Other { get; }

      public override string Description => Other.Description;

      public override IViolation<IParserResult> Check( IParserResult subject, IBehaviors context = null ) {
         return subject.IsSet( Target )
               ? NonViolation<IParserResult>.Instance
               : Other.Check( subject, context );
      }

      public override IEnumerable<ISolution<IParserResult>> MakeValid( ) {
         return Other.MakeValid( );
      }

      public override IEnumerable<ISolution<IParserResult>> MakeInvalid( ) {
         return Other.MakeInvalid( );
      }

   }

}