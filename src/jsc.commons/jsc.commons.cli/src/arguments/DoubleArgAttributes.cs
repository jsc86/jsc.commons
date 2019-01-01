// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.cli.interfaces;
using jsc.commons.cli.ispec.attrib.constraints;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace jsc.commons.cli.arguments {

   public partial class DoubleArg {

      [ArgumentConstraintAttributeHandler( typeof( MinValueAttributeHandler ) )]
      public class MinValueAttribute : ArgumentConstraintAttribute {

         public MinValueAttribute( double minValue ) {
            MinValue = minValue;
         }

         public double MinValue { get; set; }

      }

      private class MinValueAttributeHandler : IArgumentConstraintAttributeHandler {

         public void Handle( IArgument arg, ArgumentConstraintAttribute attrib ) {
            ArgumentConstraintHelper.XHandle<DoubleArg, MinValueAttribute>(
                  arg,
                  attrib,
                  ( arg2, attrib2 ) => arg2.MinValue = attrib2.MinValue );
         }

      }

      [ArgumentConstraintAttributeHandler( typeof( MaxValueAttributeHandler ) )]
      public class MaxValueAttribute : ArgumentConstraintAttribute {

         public MaxValueAttribute( double maxValue ) {
            MaxValue = maxValue;
         }

         public double MaxValue { get; set; }

      }

      private class MaxValueAttributeHandler : IArgumentConstraintAttributeHandler {

         public void Handle( IArgument arg, ArgumentConstraintAttribute attrib ) {
            ArgumentConstraintHelper.XHandle<DoubleArg, MaxValueAttribute>(
                  arg,
                  attrib,
                  ( arg2, attrib2 ) => arg2.MaxValue = attrib2.MaxValue );
         }

      }

   }

}