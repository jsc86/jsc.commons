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

   public class IntArg : Argument<int> {

      private readonly ICliConfig _config;

      static IntArg( ) {
         RuleDeriver.Instance.AddArgumentRuleDeriver(
               typeof( IntArg ),
               arg => {
                  List<IRule<IParserResult>> rules = new List<IRule<IParserResult>>( 2 );
                  if( ( (IntArg)arg ).MinValue.HasValue )
                        // ReSharper disable once PossibleInvalidOperationException
                     rules.Add( new MinValue<int>( (IntArg)arg, ( (IntArg)arg ).MinValue.Value ) );
                  if( ( (IntArg)arg ).MaxValue.HasValue )
                        // ReSharper disable once PossibleInvalidOperationException
                     rules.Add( new MaxValue<int>( (IntArg)arg, ( (IntArg)arg ).MaxValue.Value ) );
                  return rules;
               } );
      }

      public IntArg(
            string name,
            string description,
            ICliConfig config,
            bool optional = true ) :
            base( name, description, optional ) {
         _config = config;
      }

      public IntArg(
            string name,
            string description,
            ICliConfig config,
            int minValue,
            bool optional = true ) : this(
            name,
            description,
            config,
            optional ) {
         MinValue = minValue;
      }

      public IntArg(
            string name,
            string description,
            ICliConfig config,
            int minValue,
            int maxValue,
            bool optional = true ) : this(
            name,
            description,
            config,
            minValue,
            optional ) {
         MaxValue = maxValue;
      }

      public int? MinValue { get; }

      public int? MaxValue { get; }

      protected override object ParseInternal( string value ) {
         if( value == null )
            return null;

         return int.Parse( value, _config.Culture );
      }

   }

}