// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Collections.ObjectModel;

using jsc.commons.behaving.interfaces;
using jsc.commons.cli.actions;
using jsc.commons.cli.interfaces;
using jsc.commons.misc;
using jsc.commons.rc;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.rules {

   public class ArgumentIsSet : RuleBase<IParserResult> {

      private readonly Lazy<IEnumerable<ISolution<IParserResult>>> _lazyMakeInvalid;

      private readonly Lazy<IEnumerable<ISolution<IParserResult>>> _lazyMakeValid;

      private string _description;

      public ArgumentIsSet( IArgument target ) {
         Target = target;
         _lazyMakeValid = new Lazy<IEnumerable<ISolution<IParserResult>>>(
               ( ) => new ReadOnlyCollection<ISolution<IParserResult>>(
                     new ISolution<IParserResult>[] {
                           new Solution<IParserResult>(
                                 new[] {
                                       new AddArgument( target )
                                 } )
                     } ) );
         _lazyMakeInvalid = new Lazy<IEnumerable<ISolution<IParserResult>>>(
               ( ) => new ReadOnlyCollection<ISolution<IParserResult>>(
                     new ISolution<IParserResult>[] {
                           new Solution<IParserResult>(
                                 new[] {
                                       new RemoveArgument( target )
                                 } )
                     } ) );
      }

      private IArgument Target { get; }

      public override string Description => _description ??= $"argument {Target.Name} is set";

      public override IViolation<IParserResult> Check( IParserResult subject, IBehaviors context = null ) {
         if( subject.IsSet( Target ) )
            return NonViolation<IParserResult>.Instance;

         return new Violation<IParserResult>(
               this,
               MakeValid( ) );
      }

      public override IEnumerable<ISolution<IParserResult>> MakeValid( ) {
         return _lazyMakeValid.Instance;
      }

      public override IEnumerable<ISolution<IParserResult>> MakeInvalid( ) {
         return _lazyMakeInvalid.Instance;
      }

   }

}