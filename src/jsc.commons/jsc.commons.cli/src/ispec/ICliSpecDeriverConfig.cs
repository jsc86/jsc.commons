// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

using jsc.commons.cli.config;
using jsc.commons.config;
using jsc.commons.config.interfaces;
using jsc.commons.naming.interfaces;

namespace jsc.commons.cli.ispec {

   [Config( DefaultsProvider = typeof( CliSpecDeriverConfigDefaultsProvider ) )]
   public interface ICliSpecDeriverConfig : IConfiguration {

      [ConfigValue]
      INamingStyle PropertyNamingStyle { get; set; }

      [ConfigValue]
      ICliConfig CliConfig { get; set; }

      [ConfigValue]
      Dictionary<Type, Func<ICliConfig, string, bool, Argument>> ExtendedAttributeMappers { get; set; }

   }

}