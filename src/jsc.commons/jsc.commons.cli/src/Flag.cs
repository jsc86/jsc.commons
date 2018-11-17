// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Linq;

using jsc.commons.cli.interfaces;

namespace jsc.commons.cli {

   public class Flag : ItemBase, IFlag {

      public Flag(
            char name,
            bool optional = true,
            IArgument dynamicArgument = null,
            IEnumerable<IArgument> args = null ) : this(
            name,
            optional,
            dynamicArgument,
            args?.ToArray( ) ) { }

      public Flag(
            char name,
            bool optional = true,
            IArgument dynamicArgument = null,
            params IArgument[] args ) : base( optional, dynamicArgument, args ) {
         Name = name;
      }

      public char Name { get; }

   }

}