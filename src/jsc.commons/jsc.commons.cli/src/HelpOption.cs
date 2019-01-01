// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.cli.config;
using jsc.commons.naming;

namespace jsc.commons.cli {

   public class HelpOption : Option {

      public HelpOption( ICliConfig config, char? flag = null ) : base(
            new UnifiedName( "help" ),
            true,
            flag??HelpFlagFromPrefix( config.FlagPrefix( ) ),
            null,
            null ) { }

      private static char HelpFlagFromPrefix( string flagPrefix ) {
         switch( flagPrefix ) {
            case "-":
               return 'h';
            case "/":
               return '?';
            default:
               throw new Exception( $"no help flag is mapped for prefix {flagPrefix}" );
         }
      }

   }

}