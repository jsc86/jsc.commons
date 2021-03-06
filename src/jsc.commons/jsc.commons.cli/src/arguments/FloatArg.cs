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

   public partial class FloatArg : Argument<float> {

      private readonly ICliConfig _config;

      static FloatArg( ) {
         RuleDeriver.Instance.AddArgumentRuleDeriver(
               typeof( FloatArg ),
               arg => {
                  List<IRule<IParserResult>> rules = new List<IRule<IParserResult>>( 2 );
                  if( ( (FloatArg)arg ).MinValue.HasValue )
                        // ReSharper disable once PossibleInvalidOperationException
                     rules.Add( new MinValue<float>( (FloatArg)arg, ( (FloatArg)arg ).MinValue.Value ) );
                  if( ( (FloatArg)arg ).MaxValue.HasValue )
                        // ReSharper disable once PossibleInvalidOperationException
                     rules.Add( new MaxValue<float>( (FloatArg)arg, ( (FloatArg)arg ).MaxValue.Value ) );
                  return rules;
               } );
      }

      public FloatArg(
            string name,
            string description,
            ICliConfig config,
            bool optional = true ) :
            base( name, description, optional ) {
         _config = config;
      }

      public FloatArg(
            string name,
            string description,
            ICliConfig config,
            float minValue,
            bool optional = true ) : this(
            name,
            description,
            config,
            optional ) {
         MinValue = minValue;
      }

      public FloatArg(
            string name,
            string description,
            ICliConfig config,
            float minValue,
            float maxValue,
            bool optional = true ) : this(
            name,
            description,
            config,
            minValue,
            optional ) {
         MaxValue = maxValue;
      }

      public float? MinValue { get; internal set; }

      public float? MaxValue { get; internal set; }

      protected override object ParseInternal( string value ) {
         if( value == null )
            return null;

         return float.Parse( value, _config.Culture );
      }

      public override bool CanStartWithPrefix( ICliConfig conf ) {
         return ArgumentHelper.SignedNumberCanStartWithPrefixCheck( conf );
      }

   }

}