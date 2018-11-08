// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.cli.config;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.interfaces {

   public interface ICliSpecification : IItem {

      IList<IFlag> Flags { get; }

      IList<IOption> Options { get; }

      ICliConfig Config { get; }

      IList<IRule<IParserResult>> Rules { get; }

   }

}