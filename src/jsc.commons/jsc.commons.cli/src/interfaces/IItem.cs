// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

namespace jsc.commons.cli.interfaces {

   public interface IItem {

      string Description { get; }

      IEnumerable<IArgument> Arguments { get; }

      bool HasDynamicArgumentList { get; }

      bool Optional { get; }

   }

}