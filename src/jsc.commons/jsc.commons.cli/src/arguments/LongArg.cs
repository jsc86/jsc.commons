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

   public partial class LongArg : Argument<long> {

      private readonly ICliConfig _config;

      static LongArg( ) {
         RuleDeriver.Instance.AddArgumentRuleDeriver(
               typeof( LongArg ),
               arg => {
                  List<IRule<IParserResult>> rules = new List<IRule<IParserResult>>( 2 );
                  if( ( (LongArg)arg ).MinValue.HasValue )
                        // ReSharper disable once PossibleInvalidOperationException
                     rules.Add( new MinValue<long>( (LongArg)arg, ( (LongArg)arg ).MinValue.Value ) );
                  if( ( (LongArg)arg ).MaxValue.HasValue )
                        // ReSharper disable once PossibleInvalidOperationException
                     rules.Add( new MaxValue<long>( (LongArg)arg, ( (LongArg)arg ).MaxValue.Value ) );
                  return rules;
               } );
      }

      public LongArg(
            string name,
            string description,
            ICliConfig config,
            bool optional = true ) :
            base( name, description, optional ) {
         _config = config;
      }

      public LongArg(
            string name,
            string description,
            ICliConfig config,
            long minValue,
            bool optional = true ) : this(
            name,
            description,
            config,
            optional ) {
         MinValue = minValue;
      }

      public LongArg(
            string name,
            string description,
            ICliConfig config,
            long minValue,
            long maxValue,
            bool optional = true ) : this(
            name,
            description,
            config,
            minValue,
            optional ) {
         MaxValue = maxValue;
      }

      public long? MinValue { get; internal set; }

      public long? MaxValue { get; internal set; }

      protected override object ParseInternal( string value ) {
         if( value == null )
            return null;

         return long.Parse( value, _config.Culture );
      }

   }

}