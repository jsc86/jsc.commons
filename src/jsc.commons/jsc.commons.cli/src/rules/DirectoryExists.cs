// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.IO;

using jsc.commons.cli.arguments;

namespace jsc.commons.cli.rules {

   public class DirectoryExists : FileSystemItemExists {

      public DirectoryExists( FileSystemItemArgument target ) : base( target ) { }

      protected override FileSystemInfo CreateFileSystemItem( string path ) {
         return new DirectoryInfo( path );
      }

   }

}