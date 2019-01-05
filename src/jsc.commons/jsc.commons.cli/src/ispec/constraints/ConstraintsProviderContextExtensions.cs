// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.cli.interfaces;
using jsc.commons.cli.ispec.constraints.interfaces;
using jsc.commons.config.interfaces;
using jsc.commons.rc.generic.rules;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.ispec.constraints {

   public static class ConstraintsProviderContextExtensions {

      public static IRule<IParserResult> AndItemItem<T>(
            this IConstraintsProviderContext<T> cpc,
            string itemPropertyName1,
            string itemPropertyName2 )
            where T : IConfiguration {
         return new And<IParserResult>(
               cpc.ItemIsSet( itemPropertyName1 ),
               cpc.ItemIsSet( itemPropertyName2 ) );
      }

      public static IRule<IParserResult> AndItemArgument<T>(
            this IConstraintsProviderContext<T> cpc,
            string itemPropertyName,
            string argumentPropertyName )
            where T : IConfiguration {
         return new And<IParserResult>(
               cpc.ItemIsSet( itemPropertyName ),
               cpc.ArgumentIsSet( argumentPropertyName ) );
      }

      public static IRule<IParserResult> OrItemItem<T>(
            this IConstraintsProviderContext<T> cpc,
            string itemPropertyName1,
            string itemPropertyName2 )
            where T : IConfiguration {
         return new Or<IParserResult>(
               cpc.ItemIsSet( itemPropertyName1 ),
               cpc.ItemIsSet( itemPropertyName2 ) );
      }

      public static IRule<IParserResult> OrItemArgument<T>(
            this IConstraintsProviderContext<T> cpc,
            string itemPropertyName,
            string argumentPropertyName )
            where T : IConfiguration {
         return new Or<IParserResult>(
               cpc.ItemIsSet( itemPropertyName ),
               cpc.ArgumentIsSet( argumentPropertyName ) );
      }

      public static IRule<IParserResult> ImpliesItemItem<T>(
            this IConstraintsProviderContext<T> cpc,
            string itemPropertyName1,
            string itemPropertyName2 )
            where T : IConfiguration {
         return new Implies<IParserResult>(
               cpc.ItemIsSet( itemPropertyName1 ),
               cpc.ItemIsSet( itemPropertyName2 ) );
      }

      public static IRule<IParserResult> ImpliesItemArgument<T>(
            this IConstraintsProviderContext<T> cpc,
            string itemPropertyName,
            string argumentPropertyName )
            where T : IConfiguration {
         return new Implies<IParserResult>(
               cpc.ItemIsSet( itemPropertyName ),
               cpc.ArgumentIsSet( argumentPropertyName ) );
      }

      public static IRule<IParserResult> NotItem<T>(
            this IConstraintsProviderContext<T> cpc,
            string itemPropertyName )
            where T : IConfiguration {
         return new Not<IParserResult>( cpc.ItemIsSet( itemPropertyName ) );
      }

      public static IRule<IParserResult> NotArgument<T>(
            this IConstraintsProviderContext<T> cpc,
            string argumentPropertyName )
            where T : IConfiguration {
         return new Not<IParserResult>( cpc.ArgumentIsSet( argumentPropertyName ) );
      }

   }

}