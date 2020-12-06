// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Globalization;

namespace jsc.commons.config.tests {

   public class TestConfigDefaultsProvider : DefaultsProviderBase {

      public TestConfigDefaultsProvider( ) : base(
            new[] {
                  new Tuple<string, Func<object>>(
                        nameof( ITestConfig.Culture ),
                        ( ) => CultureInfo.InvariantCulture )
            } ) { }

   }

}