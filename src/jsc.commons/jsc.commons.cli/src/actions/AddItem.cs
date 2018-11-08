// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.behaving.interfaces;
using jsc.commons.cli.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.actions {

   public class AddItem : IAction<IParserResult> {

      private string _description;

      public AddItem( IItem target ) {
         Target = target;
      }

      public IItem Target { get; }

      public void Apply( IParserResult subject, IBehaviors context ) {
         ( subject as ParserResult )?.Set( Target );
      }

      public bool Contradicts( IAction<IParserResult> a ) {
         return a is RemoveItem remove&&remove.Target == Target;
      }

      public string Description =>
            _description??
            ( _description = string.Format(
                  "add {0} {1}",
                  Target is IOption? "option" : "flag",
                  ( Target as IOption )?.Name.ToString( )??( Target as IFlag )?.Name.ToString( ) ) );

      public bool IsInteractive { get; } = false;

   }

}