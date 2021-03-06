﻿// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.cli.config;
using jsc.commons.cli.interfaces;

namespace jsc.commons.cli.input {

   public static class Default {

      public static string Prompt( ICliConfig config, IParserResult pr, IArgument arg ) {
         config.Out.Write( $"{Environment.NewLine}Enter value for argument {arg.Name}: " );
         return config.In.ReadLine( );
      }

   }

}