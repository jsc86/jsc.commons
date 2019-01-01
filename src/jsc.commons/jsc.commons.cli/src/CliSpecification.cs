// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Linq;

using jsc.commons.cli.config;
using jsc.commons.cli.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli {

   public class CliSpecification : ItemBase, ICliSpecification {

      public CliSpecification(
            ICliConfig config = null,
            IArgument dynamicArgument = null,
            IEnumerable<IArgument> args = null ) : this(
            config,
            dynamicArgument,
            args?.ToArray( ) ) { }

      public CliSpecification(
            ICliConfig config = null,
            IArgument dynamicArgument = null,
            params IArgument[] args ) : base( true, dynamicArgument, args ) {
         Config = config??commons.config.Config.New<ICliConfig>( );
         if( Config.AutoAddHelpOption )
            Options.Add( HelpOption = new HelpOption( Config ) );
      }

      public IList<IFlag> Flags { get; } = new List<IFlag>( );

      public IList<IOption> Options { get; } = new List<IOption>( );

      public ICliConfig Config { get; }

      public IList<IRule<IParserResult>> Rules { get; } = new List<IRule<IParserResult>>( );

      public HelpOption HelpOption { get; }

   }

}