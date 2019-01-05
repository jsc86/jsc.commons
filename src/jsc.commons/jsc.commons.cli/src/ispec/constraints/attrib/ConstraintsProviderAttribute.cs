// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

namespace jsc.commons.cli.ispec.constraints.attrib {

   public class ConstraintsProviderAttribute : Attribute {

      public ConstraintsProviderAttribute( ) { }

      public ConstraintsProviderAttribute( Type type ) {
         Type = type;
      }

      public Type Type { get; set; }

   }

}