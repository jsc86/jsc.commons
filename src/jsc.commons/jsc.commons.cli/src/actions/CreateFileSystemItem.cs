// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.behaving.interfaces;
using jsc.commons.cli.arguments;
using jsc.commons.cli.interfaces;

namespace jsc.commons.cli.actions {

   public class CreateFileSystemItem : FileSystemItemAction {

      public CreateFileSystemItem( FileSystemItemArgument target, bool interactive ) :
            base( target, interactive ) {
         // nop
      }

      public override string Description => _description??( _description = $"create file/directory of {Target.Name}" );

      public override bool ChangesSubject( IParserResult subject ) {
         return true;
      }

      public override void Apply( IParserResult subject, IBehaviors context = null ) {
         Target.Create( subject.GetValue( Target ) );
      }

   }

}