// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.cli.config;
using jsc.commons.naming;

namespace jsc.commons.cli.interfaces {

   public interface IOption : IItem {

      UnifiedName Name { get; }

      char? FlagAlias { get; }

      string GetDeUnifiedName( ICliConfig conf );

   }

}