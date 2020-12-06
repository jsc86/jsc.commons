// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.behaving.interfaces;
using jsc.commons.cli.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.actions {

   public class RemoveArgument : IAction<IParserResult> {

      private string _description;

      public RemoveArgument( IArgument target ) {
         Target = target;
      }

      public IArgument Target { get; }

      public void Apply( IParserResult subject, IBehaviors context ) {
         ( subject as ParserResult )?.RemoveArgument( Target );
      }

      public bool Contradicts( IAction<IParserResult> a ) {
         return a is AddArgument add&&add.Target == Target;
      }

      public bool ChangesSubject( IParserResult subject ) {
         return subject.IsSet( Target );
      }

      public string Description => _description ??= $"remove argument {Target.Name}";

      public bool IsInteractive { get; } = false;

   }

}