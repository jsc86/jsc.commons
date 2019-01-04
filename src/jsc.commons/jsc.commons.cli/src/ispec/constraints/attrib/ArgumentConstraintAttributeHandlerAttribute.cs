// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

namespace jsc.commons.cli.ispec.constraints.attrib {

   [AttributeUsage( AttributeTargets.Class )]
   public class ArgumentConstraintAttributeHandlerAttribute : Attribute {

      public ArgumentConstraintAttributeHandlerAttribute( ) { }

      public ArgumentConstraintAttributeHandlerAttribute( Type handler ) {
         Handler = handler;
      }

      public Type Handler { get; set; }

   }

}