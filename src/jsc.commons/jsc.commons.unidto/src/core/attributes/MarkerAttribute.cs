// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

namespace jsc.commons.unidto.core.attributes {

   [AttributeUsage( AttributeTargets.Interface )]
   public class MarkerAttribute : Attribute {

      public MarkerAttribute( string mark ) {
         Mark = mark;
      }

      public string Mark { get; set; }

   }

}