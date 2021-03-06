// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.IO;

namespace jsc.commons.cli.arguments {

   public class FileArgument : FileSystemItemArgument {

      static FileArgument( ) {
         RegisterRuleDerivers<FileArgument>( );
      }

      public FileArgument(
            string name,
            string description,
            bool optional = true ) : base( name, description, optional ) { }

      public FileArgument(
            string name,
            string description,
            bool mustExist,
            bool optional = true ) : base( name, description, mustExist, optional ) { }

      public FileArgument(
            string name,
            string description,
            bool mustExist,
            bool mustNotExist,
            bool optional = true ) : base( name, description, mustExist, mustNotExist, optional ) { }

      protected override object ParseInternal( string value ) {
         return new FileInfo( value );
      }

      internal override void Create( FileSystemInfo fsi ) {
         ( fsi as FileInfo )?.Create( );
      }

      internal override void Delete( FileSystemInfo fsi ) {
         ( fsi as FileInfo )?.Delete( );
      }

   }

}