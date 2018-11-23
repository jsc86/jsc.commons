// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.cli.help;
using jsc.commons.cli.interfaces;
using jsc.commons.config;

namespace jsc.commons.cli.ispec {

   public class InterfaceSpecBoilerPlateHelperConfigDefaultsProvider : DefaultsProviderBase {

      protected InterfaceSpecBoilerPlateHelperConfigDefaultsProvider( ) : base(
            new[] {
                  new Tuple<string, Func<object>>(
                        nameof( IInterfaceSpecBoilerPlateHelperConfig.HelpPrinter ),
                        ( ) => (Func<ICliSpecification, ITextHelpPrinterConfig, IHelpPrinter>)
                              ( ( spec, config ) => new TextHelpPrinter( spec, config ) ) )
            } ) { }

   }

}