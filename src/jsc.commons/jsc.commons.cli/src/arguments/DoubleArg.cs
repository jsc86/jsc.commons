// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.cli.config;
using jsc.commons.cli.interfaces;
using jsc.commons.cli.rules;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.arguments {

   public partial class DoubleArg : Argument<double> {

      private readonly ICliConfig _config;

      static DoubleArg( ) {
         RuleDeriver.Instance.AddArgumentRuleDeriver(
               typeof( DoubleArg ),
               arg => {
                  List<IRule<IParserResult>> rules = new List<IRule<IParserResult>>( 2 );
                  if( ( (DoubleArg)arg ).MinValue.HasValue )
                        // ReSharper disable once PossibleInvalidOperationException
                     rules.Add( new MinValue<double>( (DoubleArg)arg, ( (DoubleArg)arg ).MinValue.Value ) );
                  if( ( (DoubleArg)arg ).MaxValue.HasValue )
                        // ReSharper disable once PossibleInvalidOperationException
                     rules.Add( new MaxValue<double>( (DoubleArg)arg, ( (DoubleArg)arg ).MaxValue.Value ) );
                  return rules;
               } );
      }

      public DoubleArg(
            string name,
            string description,
            ICliConfig config,
            bool optional = true ) :
            base( name, description, optional ) {
         _config = config;
      }

      public DoubleArg(
            string name,
            string description,
            ICliConfig config,
            double minValue,
            bool optional = true ) : this(
            name,
            description,
            config,
            optional ) {
         MinValue = minValue;
      }

      public DoubleArg(
            string name,
            string description,
            ICliConfig config,
            double minValue,
            double maxValue,
            bool optional = true ) : this(
            name,
            description,
            config,
            minValue,
            optional ) {
         MaxValue = maxValue;
      }

      public double? MinValue { get; internal set; }

      public double? MaxValue { get; internal set; }

      protected override object ParseInternal( string value ) {
         if( value == null )
            return null;

         return double.Parse( value, _config.Culture );
      }

   }

}