// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.behaving.interfaces;

namespace jsc.commons.hierarchy.resources {

   public class ReferenceMeta : IBehavior {

      public string Path { get; set; }

      public override string ToString( ) {
         return $"meta: reference path {Path}";
      }

   }

}
