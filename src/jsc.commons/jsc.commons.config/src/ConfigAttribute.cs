// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

namespace jsc.commons.config {

   [AttributeUsage( AttributeTargets.Interface )]
   public class ConfigAttribute : Attribute {

      public Type DefaultsProvider { get; set; }

   }

}