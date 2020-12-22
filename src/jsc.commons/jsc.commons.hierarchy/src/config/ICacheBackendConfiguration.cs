// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Specialized;
using System.Runtime.Caching;

using jsc.commons.config;
using jsc.commons.hierarchy.backend;
using jsc.commons.hierarchy.backend.interfaces;

namespace jsc.commons.hierarchy.config {

   [Config( DefaultsProvider = typeof( CacheBackendConfigurationDefaultsProvider ) )]
   public interface ICacheBackendConfiguration : IBackendConfiguration {

      [ConfigValue]
      NameValueCollection MemoryCacheConfiguration { get; set; }

      [ConfigValue]
      CacheItemPolicy CacheItemPolicy { get; set; }

      [ConfigValue]
      IBackendConfiguration NestedBackendConfiguration { get; set; }

   }

   public class CacheBackendConfigurationDefaultsProvider : DefaultsProviderBase {

      public CacheBackendConfigurationDefaultsProvider( ) : base(
            new[] {
                  new Tuple<string, Func<object>>(
                        nameof( IBackendConfiguration.BackendType ),
                        ( ) => typeof( CacheBackend ) ),
                  new Tuple<string, Func<object>>(
                        nameof( IBackendConfiguration.BackendFactory ),
                        ( ) => (Func<IHierarchyConfiguration, IBackendConfiguration, IHierarchyBackend>)
                              ( ( config, backendConfig ) => new CacheBackend(
                                    config,
                                    (ICacheBackendConfiguration)backendConfig ) )
                  ),
                  new Tuple<string, Func<object>>(
                        nameof( ICacheBackendConfiguration.MemoryCacheConfiguration ),
                        ( ) => new NameValueCollection( ) ),
                  new Tuple<string, Func<object>>(
                        nameof( ICacheBackendConfiguration.CacheItemPolicy ),
                        ( ) => new CacheItemPolicy( ) )
            } ) { }

   }

}
