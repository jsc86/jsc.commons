// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.cli.config;
using jsc.commons.config;
using jsc.commons.naming.styles;

namespace jsc.commons.cli.ispec {

   public class CliSpecDeriverConfigDefaultsProvider : DefaultsProviderBase {

      public CliSpecDeriverConfigDefaultsProvider( ) : base(
            new[] {
                  new Tuple<string, Func<object>>(
                        nameof( ICliSpecDeriverConfig.PropertyNamingStyle ),
                        ( ) => NamingStyles.PascalCase ),
                  new Tuple<string, Func<object>>(
                        nameof( ICliSpecDeriverConfig.CliConfig ),
                        Config.New<ICliConfig> )
            } ) { }

   }

}
