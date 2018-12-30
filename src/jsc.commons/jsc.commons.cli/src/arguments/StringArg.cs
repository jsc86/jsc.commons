// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

namespace jsc.commons.cli.arguments {

   public class StringArg : Argument<string> {

      public StringArg(
            string name,
            string description,
            bool optional = true ) :
            base( name, description, optional ) {
         DefaultValue = string.Empty;
      }

   }

}