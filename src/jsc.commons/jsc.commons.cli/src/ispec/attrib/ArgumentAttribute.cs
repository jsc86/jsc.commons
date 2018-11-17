// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.config;

namespace jsc.commons.cli.ispec.attrib {

   [AttributeUsage( AttributeTargets.Property )]
   public class ArgumentAttribute : ConfigValueAttribute {

      public int Order { get; set; } = -1;

      public string Name { get; set; }

      public bool Optional { get; set; } = true;

      public string Of { get; set; }

      public string Description { get; set; }

      public bool Dynamic { get; set; }

   }

}