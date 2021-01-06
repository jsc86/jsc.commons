// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

namespace jsc.commons.hierarchy.meta {

   [AttributeUsage( AttributeTargets.Class )]
   public class MetaAutoModHandlerAttribute : Attribute {

      public MetaAutoModHandlerAttribute( Type metaAutoModHandlerType ) {
         MetaAutoModHandlerType = metaAutoModHandlerType;
      }

      public Type MetaAutoModHandlerType { get; set; }

   }

}