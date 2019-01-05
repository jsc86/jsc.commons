// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.cli.interfaces;
using jsc.commons.config.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.ispec.constraints.interfaces {

   public interface IConstraintsProviderContext<T>
         where T : IConfiguration {

      IEnumerable<IRule<IParserResult>> Rules { get; }

      IRule<IParserResult> ItemIsSet( string propertyName );

      IRule<IParserResult> ArgumentIsSet( string propertyName );

      void Add( IRule<IParserResult> rule );

   }

}