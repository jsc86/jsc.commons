// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

using jsc.commons.cli.input;
using jsc.commons.cli.interfaces;
using jsc.commons.cli.policies;
using jsc.commons.config;
using jsc.commons.naming.styles;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.config {

   public class CliConfigDefaultsProvider : DefaultsProviderBase {

      public CliConfigDefaultsProvider( ) : base(
            new[] {
                  new Tuple<string, object>( nameof( ICliConfig.Culture ), CultureInfo.InvariantCulture ),
                  new Tuple<string, object>( nameof( ICliConfig.OptionNamingStyle ), new DelimitedCaseBase( '-' ) ),
                  new Tuple<string, object>(
                        nameof( ICliConfig.PathSeparator ),
                        Path.PathSeparator ),
                  new Tuple<string, object>( nameof( ICliConfig.FlagPrefix ), (Func<string>)( ( ) => "-" ) ),
                  new Tuple<string, object>( nameof( ICliConfig.OptionPrefix ), (Func<string>)( ( ) => "--" ) ),
                  new Tuple<string, object>(
                        nameof( ICliConfig.Policies ),
                        new ReadOnlyCollection<IRule<ICliSpecification>>(
                              new List<IRule<ICliSpecification>> {
                                    new UniqueNames( )
                              } ) ),
                  new Tuple<string, object>(
                        nameof( ICliConfig.Prompt ),
                        (Func<IParserResult, IArgument, string>)Default.Prompt )
            } ) { }

   }

}
