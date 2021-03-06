﻿// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.behaving.interfaces;
using jsc.commons.cli.config;
using jsc.commons.cli.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.actions {

   public class PromptValue<T> : IAction<IParserResult> {

      private string _description;

      public PromptValue( IArgument<T> target ) {
         Target = target;
      }

      public IArgument<T> Target { get; }

      public void Apply( IParserResult subject, IBehaviors context ) {
         ICliConfig config = context.Get<ConfigBehavior>( ).Config;
         ( subject as ParserResult )?.SetArgument(
               Target,
               config.Prompt( config, subject, Target ) );
      }

      public bool Contradicts( IAction<IParserResult> a ) {
         return a is PromptValue<T> promptVal&&promptVal.Target == Target||
               a is SetValue<T> setVal&&setVal.Target == Target;
      }

      public bool ChangesSubject( IParserResult subject ) {
         return true;
      }

      public string Description =>
            _description ??= $"enter value for argument {Target.Name}";


      public bool IsInteractive { get; } = true;

   }

}