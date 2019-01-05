// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using jsc.commons.cli.interfaces;
using jsc.commons.cli.ispec.constraints.attrib;
using jsc.commons.cli.ispec.constraints.interfaces;
using jsc.commons.config.interfaces;
using jsc.commons.misc;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.ispec.constraints {

   public class ConstraintsCollector<T>
         where T : IConfiguration {

      private readonly ICliSpecification _spec;

      private readonly ICliSpecDeriverConfig _specDeriverConfig;
      private readonly Type _t = typeof( T );
      private List<IRule<IParserResult>> _rules;

      public ConstraintsCollector(
            ICliSpecDeriverConfig specDeriverConfig,
            ICliSpecification spec ) {
         _specDeriverConfig = specDeriverConfig;
         _spec = spec;
      }

      public IEnumerable<IRule<IParserResult>> Constraints {
         get {
            if( _rules == null )
               Collect( );

            return new EnumerableWrapper<IRule<IParserResult>>( _rules );
         }
      }

      private void Collect( ) {
         ConstraintsProviderAttribute cpa = _t.GetCustomAttribute<ConstraintsProviderAttribute>( );
         if( cpa == null ) {
            _rules = new List<IRule<IParserResult>>( 0 );
            return;
         }

         IConstraintsProvider<T> cp = (IConstraintsProvider<T>)Activator.CreateInstance( cpa.Type );
         IConstraintsProviderContext<T> cpc = new ConstraintsProviderContext<T>( _spec, _specDeriverConfig );

         cp.ProvideConstraints( cpc );

         _rules = cpc.Rules.ToList( );
      }

   }

}