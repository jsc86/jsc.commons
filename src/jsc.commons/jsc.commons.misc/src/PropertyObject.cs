// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

namespace jsc.commons.misc {

   public class PropertyObject<T> {

      public PropertyObject( ) {
         // nop
      }

      public PropertyObject( T value ) {
         Value = value;
      }

      public T Value { get; set; }

   }

}