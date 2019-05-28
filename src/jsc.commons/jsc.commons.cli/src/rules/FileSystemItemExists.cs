// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.IO;

using jsc.commons.behaving.interfaces;
using jsc.commons.cli.actions;
using jsc.commons.cli.arguments;
using jsc.commons.cli.interfaces;
using jsc.commons.rc;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.rules {

   public abstract class FileSystemItemExists : RuleBase<IParserResult> {

      public FileSystemItemExists( FileSystemItemArgument target ) {
         Target = target;
      }

      public FileSystemItemArgument Target { get; }

      public override IViolation<IParserResult> Check( IParserResult subject, IBehaviors context = null ) {
         if( !subject.IsSet( Target ) )
            return NonViolation<IParserResult>.Instance;

         string path = subject.GetValue( (IArgument)Target );
         FileSystemInfo fsi;
         try {
            fsi = CreateFileSystemItem( path );
         } catch( Exception ) {
            // invalid path format
            return new Violation<IParserResult>(
                  this,
                  new[] {
                        new Solution<IParserResult>(
                              new[] {
                                    new PromptValue<FileSystemInfo>( Target )
                              } )
                  } );
         }

         if( fsi.Exists )
            return NonViolation<IParserResult>.Instance;

         return new Violation<IParserResult>(
               this,
               new[] {
                     new Solution<IParserResult>(
                           new[] {
                                 new CreateFileSystemItem( Target, Target.Interactive )
                           } ),
                     new Solution<IParserResult>(
                           new[] {
                                 new PromptValue<FileSystemInfo>( Target )
                           } ),
                     new Solution<IParserResult>(
                           new[] {
                                 new RemoveArgument( Target )
                           } )
               } );
      }

      protected abstract FileSystemInfo CreateFileSystemItem( string path );

   }

}
