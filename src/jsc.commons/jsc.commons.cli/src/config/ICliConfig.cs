﻿// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using jsc.commons.cli.interfaces;
using jsc.commons.config;
using jsc.commons.config.interfaces;
using jsc.commons.naming.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.config {

   [Config( DefaultsProvider = typeof( CliConfigDefaultsProvider ) )]
   public interface ICliConfig : IConfiguration {

      [ConfigValue]
      CultureInfo Culture { get; set; }

      [ConfigValue]
      INamingStyle OptionNamingStyle { get; set; }

      [ConfigValue]
      Func<string> FlagPrefix { get; set; }

      [ConfigValue( Default = true )]
      bool CaseSensitiveFlags { get; set; }

      [ConfigValue]
      Func<string> OptionPrefix { get; set; }

      [ConfigValue( Default = true )]
      bool CaseSensitiveOptions { get; set; }

      [ConfigValue]
      string PathSeparator { get; set; }

      [ConfigValue]
      IReadOnlyList<IRule<ICliSpecification>> Policies { get; set; }

      [ConfigValue]
      Func<ICliConfig, IParserResult, IArgument, string> Prompt { get; set; }

      [ConfigValue( Default = true )]
      bool AutoAddHelpOption { get; set; }

      [ConfigValue]
      TextWriter Out { get; set; }

      [ConfigValue]
      TextReader In { get; set; }

   }

}