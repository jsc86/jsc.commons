// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

namespace jsc.commons.hierarchy.path.interfaces {

   public interface IPath : IComparable, IComparable<IPath> {

      bool Absolute { get; }

      IEnumerable<string> Elements { get; }

      string Name { get; }

      IPath BasePath { get; }

   }

}