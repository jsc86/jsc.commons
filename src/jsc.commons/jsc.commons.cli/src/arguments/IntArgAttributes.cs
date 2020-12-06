// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.cli.interfaces;
using jsc.commons.cli.ispec.constraints;
using jsc.commons.cli.ispec.constraints.attrib;
using jsc.commons.cli.ispec.constraints.interfaces;

namespace jsc.commons.cli.arguments {

   public partial class IntArg {

      [ArgumentConstraintAttributeHandler( typeof( MinValueAttributeHandler ) )]
      public class MinValueAttribute : ArgumentConstraintAttribute {

         public MinValueAttribute( int minValue ) {
            MinValue = minValue;
         }

         public int MinValue { get; set; }

      }

      private class MinValueAttributeHandler : IArgumentConstraintAttributeHandler {

         public void Handle( IArgument arg, ArgumentConstraintAttribute attrib ) {
            ArgumentConstraintHelper.XHandle<IntArg, MinValueAttribute>(
                  arg,
                  attrib,
                  ( arg2, attrib2 ) => arg2.MinValue = attrib2.MinValue );
         }

      }

      [ArgumentConstraintAttributeHandler( typeof( MaxValueAttributeHandler ) )]
      public class MaxValueAttribute : ArgumentConstraintAttribute {

         public MaxValueAttribute( int maxValue ) {
            MaxValue = maxValue;
         }

         public int MaxValue { get; set; }

      }

      private class MaxValueAttributeHandler : IArgumentConstraintAttributeHandler {

         public void Handle( IArgument arg, ArgumentConstraintAttribute attrib ) {
            ArgumentConstraintHelper.XHandle<IntArg, MaxValueAttribute>(
                  arg,
                  attrib,
                  ( arg2, attrib2 ) => arg2.MaxValue = attrib2.MaxValue );
         }

      }

   }

}