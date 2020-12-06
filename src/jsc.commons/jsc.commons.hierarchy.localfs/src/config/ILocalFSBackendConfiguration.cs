using System;
using System.IO;

using jsc.commons.config;
using jsc.commons.hierarchy.backend.interfaces;
using jsc.commons.hierarchy.config;

namespace jsc.commons.hierarchy.localfs.config {

   [Config( DefaultsProvider = typeof( LocalFsBackendConfigurationDefaultsProvider ) )]
   public interface ILocalFsBackendConfiguration : IBackendConfiguration {

      [ConfigValue]
      DirectoryInfo BasePath { get; set; }

      [ConfigValue( Default = ".meta" )]
      string MetaSuffix { get; set; }

      [ConfigValue( Default = false )]
      bool CreateBasePath { get; set; }

   }

   public class LocalFsBackendConfigurationDefaultsProvider : DefaultsProviderBase {

      public LocalFsBackendConfigurationDefaultsProvider( ) : base(
            new[] {
                  new Tuple<string, Func<object>>(
                        nameof( IBackendConfiguration.BackendType ),
                        ( ) => typeof( LocalFsBackend ) ),
                  new Tuple<string, Func<object>>(
                        nameof( IBackendConfiguration.BackendFactory ),
                        ( ) => (Func<IHierarchyConfiguration, IHierarchyBackend>)
                              ( config => new LocalFsBackend( config ) )
                  ),
                  new Tuple<string, Func<object>>(
                        nameof( ILocalFsBackendConfiguration.BasePath ),
                        ( ) => new DirectoryInfo( "./hierarchy" ) )
            } ) { }

   }

}