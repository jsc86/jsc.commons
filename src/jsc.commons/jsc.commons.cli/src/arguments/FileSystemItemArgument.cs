// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.IO;

using jsc.commons.cli.interfaces;
using jsc.commons.cli.rules;
using jsc.commons.rc.generic.rules;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.arguments {

   public abstract class FileSystemItemArgument : Argument<FileSystemInfo> {

      public FileSystemItemArgument(
            string name,
            string description,
            bool optional = true ) : base( name, description, optional ) { }

      public FileSystemItemArgument(
            string name,
            string description,
            bool mustExist,
            bool optional = true ) : this( name, description, optional ) {
         MustExist = mustExist;
      }

      public FileSystemItemArgument(
            string name,
            string description,
            bool mustExist,
            bool mustNotExist,
            bool optional = true ) : this( name, description, mustExist, optional ) {
         MustNotExist = mustNotExist;
      }

      public bool MustExist { get; internal set; }

      public bool MustNotExist { get; internal set; }

      public bool Interactive { get; internal set; }

      protected static void RegisterRuleDerivers<T>( ) where T : FileSystemItemArgument {
         bool file = typeof( T ).IsAssignableFrom( typeof( FileArgument ) );
         RuleDeriver.Instance.AddArgumentRuleDeriver(
               typeof( T ),
               arg => {
                  List<IRule<IParserResult>> rules = new List<IRule<IParserResult>>( );
                  FileSystemItemArgument fsia = (FileSystemItemArgument)arg;
                  if( fsia.MustExist )
                     rules.Add( file? (FileSystemItemExists)new FileExists( fsia ) : new DirectoryExists( fsia ) );
                  if( fsia.MustNotExist )
                     rules.Add(
                           new Not<IParserResult>(
                                 file? (FileSystemItemExists)new FileExists( fsia ) : new DirectoryExists( fsia ) ) );
                  return rules;
               } );
      }

      protected abstract override object ParseInternal( string value );

      internal abstract void Create( FileSystemInfo fsi );

      internal abstract void Delete( FileSystemInfo fsi );

   }

}