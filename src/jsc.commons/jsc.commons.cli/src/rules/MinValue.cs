﻿// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using jsc.commons.behaving.interfaces;
using jsc.commons.cli.actions;
using jsc.commons.cli.interfaces;
using jsc.commons.rc;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.rules {

   public class MinValue<T> : RuleBase<IParserResult> where T : struct {

      private readonly misc.Lazy<IEnumerable<ISolution<IParserResult>>> _lazyMakeInvalid;

      private readonly misc.Lazy<IEnumerable<ISolution<IParserResult>>> _lazyMakeValid;

      private string _description;

      public MinValue( IArgument<T> target, T minValue ) {
         Target = target;
         MinVal = minValue;
         _lazyMakeValid = new misc.Lazy<IEnumerable<ISolution<IParserResult>>>(
               ( ) => new ReadOnlyCollection<ISolution<IParserResult>>(
                     new List<ISolution<IParserResult>> {
                           new Solution<IParserResult>(
                                 new[] {
                                       new SetValue<T>( target, minValue )
                                 } ),
                           new Solution<IParserResult>(
                                 new[] {
                                       new PromptValue<T>( target )
                                 } )
                     } ) );
         _lazyMakeInvalid = new misc.Lazy<IEnumerable<ISolution<IParserResult>>>(
               ( ) => new ReadOnlyCollection<ISolution<IParserResult>>(
                     new List<ISolution<IParserResult>> {
                           new Solution<IParserResult>(
                                 new[] {
                                       new SetValue<T>(
                                             target,
                                             SetValue<T>.Sub( minValue, (T)Convert.ChangeType( 1, typeof( T ) ) ) )
                                 } )
                     } ) );
      }

      public IArgument<T> Target { get; }

      public T MinVal { get; }

      public override string Description => _description??( _description = $"{Target.Name} >= {MinVal}" );

      public override IViolation<IParserResult> Check( IParserResult subject, IBehaviors context = null ) {
         try {
            if( !subject.IsSet( Target )
                  ||Comparer<T>.Default.Compare( subject.GetValue( Target ), MinVal ) >= 0 )
               return NonViolation<IParserResult>.Instance;
         } catch( Exception ) {
            // the value might simply be invalid at this point
            return NonViolation<IParserResult>.Instance;
         }

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