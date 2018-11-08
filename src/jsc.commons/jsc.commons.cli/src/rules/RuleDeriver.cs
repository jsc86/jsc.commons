// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using jsc.commons.cli.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.rules {

   public class RuleDeriver {

      private static readonly Lazy<RuleDeriver> __lazyInstance = new Lazy<RuleDeriver>( ( ) => new RuleDeriver( ) );

      private readonly Dictionary<Type, Func<IArgument, IEnumerable<IRule<IParserResult>>>> _argumentRuleDerivers =
            new Dictionary<Type, Func<IArgument, IEnumerable<IRule<IParserResult>>>>( );

      private readonly Dictionary<Type, Func<IItem, IEnumerable<IRule<IParserResult>>>> _itemRuleDerivers =
            new Dictionary<Type, Func<IItem, IEnumerable<IRule<IParserResult>>>>( );

      private RuleDeriver( ) { }

      public static RuleDeriver Instance => __lazyInstance.Value;


      internal void AddItemRuleDeriver( Type t, Func<IItem, IEnumerable<IRule<IParserResult>>> deriver ) {
         if( t == null )
            throw new NullReferenceException( $"{nameof( t )} must not be null" );
         if( deriver == null )
            throw new NullReferenceException( $"{nameof( deriver )} must not be null" );

         _itemRuleDerivers.Add( t, deriver );
      }

      internal void AddArgumentRuleDeriver( Type t, Func<IArgument, IEnumerable<IRule<IParserResult>>> deriver ) {
         if( t == null )
            throw new NullReferenceException( $"{nameof( t )} must not be null" );
         if( deriver == null )
            throw new NullReferenceException( $"{nameof( deriver )} must not be null" );

         _argumentRuleDerivers.Add( t, deriver );
      }

      public IEnumerable<IRule<IParserResult>> DeriveRules( ICliSpecification spec ) {
         List<IRule<IParserResult>> rules = new List<IRule<IParserResult>>( );
         List<IItem> items = spec.Flags.Cast<IItem>( ).Union( spec.Options ).ToList( );
         List<IArgument> args = spec.Arguments.Union( items.SelectMany( i => i.Arguments ) ).ToList( );

         foreach( IItem item in items )
            rules.AddRange(
                  _itemRuleDerivers.Where( kvp => kvp.Key.IsInstanceOfType( item ) )
                        .SelectMany( kvp => kvp.Value( item ) ) );

         foreach( IArgument arg in args )
            rules.AddRange(
                  _argumentRuleDerivers.Where( kvp => kvp.Key.IsInstanceOfType( arg ) )
                        .SelectMany( kvp => kvp.Value( arg ) ) );

         return new ReadOnlyCollection<IRule<IParserResult>>( rules );
      }

   }

}