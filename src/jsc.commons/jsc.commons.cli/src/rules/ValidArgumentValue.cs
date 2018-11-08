// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;

using jsc.commons.behaving.interfaces;
using jsc.commons.cli.actions;
using jsc.commons.cli.interfaces;
using jsc.commons.rc;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.rules {

   public class ValidArgumentValue<T> : RuleBase<IParserResult> {

      private string _description;

      public ValidArgumentValue( IArgument<T> target ) {
         Target = target;
      }

      public IArgument<T> Target { get; }

      public override string Description =>
            _description??( _description = $"valid {Target.ValueType.Name} value for {Target}" );

      public override IViolation<IParserResult> Check( IParserResult subject, IBehaviors context = null ) {
         if( !subject.IsSet( Target ) )
            return NonViolation<IParserResult>.Instance;

         try {
            Target.Parse( subject.GetValue( (IArgument)Target ), true );
         } catch( Exception exc ) {
            return new Violation<IParserResult>(
                  this,
                  new List<ISolution<IParserResult>> {
                        new Solution<IParserResult>(
                              new[] {
                                    new PromptValue<T>( Target )
                              } )
                  }.Union(
                        Target.HasDefaultValue||Target.ValueType.IsValueType
                              ? new List<ISolution<IParserResult>> {
                                    new Solution<IParserResult>(
                                          new IAction<IParserResult>[] {
                                                new SetValue<T>(
                                                      Target,
                                                      Target.HasDefaultValue
                                                            ? Target.DefaultValue
                                                            : Activator.CreateInstance<T>( ) )
                                          } )
                              }
                              : Enumerable.Empty<ISolution<IParserResult>>( ) ),
                  $"invalid value: {exc.Message}" );
         }

         return NonViolation<IParserResult>.Instance;
      }

   }

}