// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.config;

namespace jsc.commons.cli.ispec.attrib {

   [AttributeUsage( AttributeTargets.Property )]
   public class HelpAttribute : ConfigValueAttribute {

      public char Flag { get; set; } = (char)0;

      public string Description { get; set; }

   }

}