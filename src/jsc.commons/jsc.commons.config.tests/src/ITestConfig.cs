// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Globalization;

using jsc.commons.config.interfaces;

namespace jsc.commons.config.tests {

   [Config( DefaultsProvider = typeof( TestConfigDefaultsProvider ) )]
   public interface ITestConfig : IConfiguration {

      [ConfigValue]
      string StringProp1 { get; set; }

      [ConfigValue( Default = "Hello World!" )]
      string StringProp2 { get; set; }

      [ConfigValue( Default = 42 )]
      int IntProp { get; set; }

      [ConfigValue] // default set by TestConfigDefaultsProvider
      CultureInfo Culture { get; set; }

   }

}