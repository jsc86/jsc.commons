// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using Castle.DynamicProxy;

using jsc.commons.config.interfaces;
using jsc.commons.misc;

namespace jsc.commons.config {

   public class Config {

      private static ProxyGenerator _pg;
      private static readonly Lazy<ProxyGenerator> __lazyPg;
      private static readonly ProxyGenerationOptions __pgo;

      static Config( ) {
         __lazyPg = new Lazy<ProxyGenerator>( );
         __pgo = new ProxyGenerationOptions {
               Selector = ConfigInterceptorSelector.Instance
         };
      }

      public static ProxyGenerator ProxyGenerator {
         get => _pg??__lazyPg.Instance;
         set => _pg = value;
      }

      public static T New<T>( ) where T : IConfiguration {
         ConfigBase cb = new ConfigBase( );
         return (T)ProxyGenerator.CreateInterfaceProxyWithoutTarget(
               typeof( IConfiguration ),
               new[] {
                     typeof( T )
               },
               __pgo,
               new ConfigBaseInterceptor( cb ),
               new ConfigPropertyInterceptor( typeof( T ), cb ) );
      }

   }

}