// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.behaving.interfaces;
using jsc.commons.cli.arguments;
using jsc.commons.cli.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.actions {

   public abstract class FileSystemItemAction : IAction<IParserResult> {

      protected string _description;

      public FileSystemItemAction( FileSystemItemArgument target, bool interactive ) {
         Target = target;
         IsInteractive = interactive;
      }

      public FileSystemItemArgument Target { get; }

      public abstract void Apply( IParserResult subject, IBehaviors context = null );

      public bool Contradicts( IAction<IParserResult> a ) {
         switch( a ) {
            case RemoveArgument ra when ra.Target == Target:
            case DeleteFileSystemItem dfsi when dfsi.Target == Target:
            case CreateFileSystemItem cfsi when cfsi.Target == Target:
               return true;
            default:
               return false;
         }
      }

      public abstract string Description { get; }

      public bool IsInteractive { get; }

   }

}