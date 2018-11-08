// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.config;

namespace jsc.commons.cli.ispec.attrib {

   [AttributeUsage( AttributeTargets.Property )]
   public class FlagAttribute : ConfigValueAttribute {

      public char Name { get; set; }

      public bool Optional { get; set; } = true;

      public string Description { get; set; }

   }

}