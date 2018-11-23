// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

namespace jsc.commons.cli.ispec.attrib {

   [AttributeUsage( AttributeTargets.Property )]
   public class FirstArgumentAttribute : Attribute {

      public string Name { get; set; }

      public string Description { get; set; }

      public bool Dynamic { get; set; }

   }

}