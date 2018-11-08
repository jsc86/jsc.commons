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

   public class ItemIsSet : RuleBase<IParserResult> {

      private readonly Lazy<IEnumerable<ISolution<IParserResult>>> _lazyMakeInvalid;

      private readonly Lazy<IEnumerable<ISolution<IParserResult>>> _lazyMakeValid;

      private string _description;

      public ItemIsSet( IItem target ) {
         Target = target;
         _lazyMakeValid = new Lazy<IEnumerable<ISolution<IParserResult>>>(
               ( ) => new ReadOnlyCollection<ISolution<IParserResult>>(
                     new ISolution<IParserResult>[] {
                           new Solution<IParserResult>(
                                 new[] {
                                       new AddItem( Target )
                                 } )
                     } ) );
         _lazyMakeInvalid = new Lazy<IEnumerable<ISolution<IParserResult>>>(
               ( ) => new ReadOnlyCollection<ISolution<IParserResult>>(
                     new ISolution<IParserResult>[] {
                           new Solution<IParserResult>(
                                 new[] {
                                       new RemoveItem( Target )
                                 } )
                     } ) );
      }

      public IItem Target { get; }

      public override string Description =>
            _description??
            ( _description = string.Format(
                  "{0} {1} is set",
                  Target is IOption? "option" : "flag",
                  ( Target as IOption )?.Name.ToString( )??( Target as IFlag )?.Name.ToString( ) ) );

      public override IViolation<IParserResult> Check( IParserResult subject, IBehaviors context = null ) {
         if( subject.IsSet( Target ) )
            return NonViolation<IParserResult>.Instance;

         return new Violation<IParserResult>(
               this,
               MakeValid( )
         );
      }

      public override IEnumerable<ISolution<IParserResult>> MakeValid( ) {
         return _lazyMakeValid.Instance;
      }

      public override IEnumerable<ISolution<IParserResult>> MakeInvalid( ) {
         return _lazyMakeInvalid.Instance;
      }

   }

}