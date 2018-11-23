// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

using jsc.commons.cli.help;
using jsc.commons.cli.interfaces;
using jsc.commons.config;
using jsc.commons.config.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.ispec {

   [Config( DefaultsProvider = typeof( InterfaceSpecBoilerPlateHelperConfigDefaultsProvider ) )]
   public interface IInterfaceSpecBoilerPlateHelperConfig : IConfiguration {

      [ConfigValue]
      Func<IList<ISolution<IParserResult>>, ISolution<IParserResult>> UserPrompt { get; set; }

      [ConfigValue]
      Func<ICliSpecification, ITextHelpPrinterConfig, IHelpPrinter> HelpPrinter { get; set; }

   }

}