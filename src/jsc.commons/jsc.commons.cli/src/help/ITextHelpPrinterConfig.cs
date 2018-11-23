// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.IO;

using jsc.commons.config;
using jsc.commons.config.interfaces;

namespace jsc.commons.cli.help {

   [Config( DefaultsProvider = typeof( TextHelpPrinterConfigDefaultsProvider ) )]
   public interface ITextHelpPrinterConfig : IConfiguration {

      [ConfigValue]
      TextWriter TextWriter { get; set; }

   }

}