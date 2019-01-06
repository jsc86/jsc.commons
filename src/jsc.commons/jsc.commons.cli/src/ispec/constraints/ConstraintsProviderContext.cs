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
using jsc.commons.cli.ispec.attrib;
using jsc.commons.cli.ispec.constraints.interfaces;
using jsc.commons.cli.rules;
using jsc.commons.config.interfaces;
using jsc.commons.misc;
using jsc.commons.naming;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.ispec.constraints {

   public class ConstraintsProviderContext<T> : IConstraintsProviderContext<T>
         where T : IConfiguration {

      private readonly Dictionary<string, IRule<IParserResult>> _argumentRules;
      private readonly Dictionary<string, IRule<IParserResult>> _itemRules;
      private readonly List<IRule<IParserResult>> _rules;

      private readonly ICliSpecification _spec;
      private readonly ICliSpecDeriverConfig _specDeriverConfig;
      private readonly Type _t = typeof( T );

      public ConstraintsProviderContext( ICliSpecification spec, ICliSpecDeriverConfig specDeriverConfig ) {
         _spec = spec;
         _specDeriverConfig = specDeriverConfig;
         _itemRules = new Dictionary<string, IRule<IParserResult>>( );
         _argumentRules = new Dictionary<string, IRule<IParserResult>>( );
         _rules = new List<IRule<IParserResult>>( );
      }

      public IRule<IParserResult> ItemIsSet( string propertyName ) {
         if( _itemRules.TryGetValue( propertyName, out IRule<IParserResult> itemRule ) )
            return itemRule;

         PropertyInfo pi = _t.GetProperty( propertyName );
         if( pi == null )
            throw new Exception( "todo no property" );

         OptionAttribute oa = pi.GetCustomAttribute<OptionAttribute>( );
         FlagAttribute fa = pi.GetCustomAttribute<FlagAttribute>( );
         if( oa != null ) {
            UnifiedName optionName =
                  _specDeriverConfig.PropertyNamingStyle.FromString(
                        string.IsNullOrEmpty( oa.Name )? pi.Name : oa.Name );
            IOption opt = _spec.Options.FirstOrDefault( o => o.Name.Equals( optionName ) );
            if( opt == null )
               throw new Exception( "todo no option" );
            itemRule = new ItemIsSet( opt );
         } else if( fa != null ) {
            IFlag flag = _spec.Flags.FirstOrDefault( f => f.Name == fa.Name );
            if( flag == null )
               throw new Exception( "todo no flag" );
            itemRule = new ItemIsSet( flag );
         } else {
            throw new Exception( "nothing for prop name" );
         }

         _itemRules[ propertyName ] = itemRule;
         return itemRule;
      }

      public IRule<IParserResult> ArgumentIsSet( string propertyName ) {
         if( _argumentRules.TryGetValue( propertyName, out IRule<IParserResult> argumentRule ) )
            return argumentRule;

         PropertyInfo pi = _t.GetProperty( propertyName );
         if( pi == null )
            throw new Exception( "todo no property" );

         ArgumentAttribute aa = pi.GetCustomAttribute<ArgumentAttribute>( );
         FirstArgumentAttribute fa = pi.GetCustomAttribute<FirstArgumentAttribute>( );
         OptionAttribute oa = pi.GetCustomAttribute<OptionAttribute>( );
         string argName = null;
         if( aa != null )
            argName = !string.IsNullOrEmpty( aa.Name )
                  ? aa.Name
                  : string.IsNullOrEmpty( aa.Of )
                        ? propertyName
                        : propertyName.Replace( aa.Of, string.Empty );
         else if( fa != null )
            argName = !string.IsNullOrEmpty( fa.Name )
                  ? fa.Name
                  : $"{propertyName}{CliSpecDeriver.ImplicitFirstArg}";
         else if( oa != null )
            argName = $"{propertyName}{CliSpecDeriver.ImplicitFirstArg}";
         else
            throw new Exception( "todo no arg" );

         IArgument arg = _spec.Arguments.Union(
                     _spec.Options.SelectMany( o => o.Arguments ) )
               .Union(
                     _spec.Flags.SelectMany( f => f.Arguments ) )
               .FirstOrDefault( a => a.Name == argName );
         if( arg == null )
            throw new Exception( "todo no arg" );

         argumentRule = new ArgumentIsSet( arg );

         _argumentRules[ propertyName ] = argumentRule;
         return argumentRule;
      }

      public void Add( IRule<IParserResult> rule ) {
         _rules.Add( rule );
      }

      public IEnumerable<IRule<IParserResult>> Rules =>
            new EnumerableWrapper<IRule<IParserResult>>( _rules );

   }

}