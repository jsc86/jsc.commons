// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.cli.interfaces;
using jsc.commons.cli.ispec.attrib;
using jsc.commons.config;
using jsc.commons.config.interfaces;

namespace jsc.commons.cli.ispec {

   public class ParserResultMapper {

      private readonly ICliSpecification _spec;

      private readonly ICliSpecDeriverConfig _specDeriverConfig;

      public ParserResultMapper(
            ICliSpecDeriverConfig specDeriverConfig,
            ICliSpecification spec ) {
         if( specDeriverConfig == null )
            throw new ArgumentException(
                  $"{nameof( specDeriverConfig )} must not be null",
                  nameof( specDeriverConfig ) );
         if( spec == null )
            throw new ArgumentException( $"{nameof( spec )} must not be null", nameof( spec ) );

         _specDeriverConfig = specDeriverConfig;
         _spec = spec;
      }

      public T Map<T>( IParserResult pr ) where T : IConfiguration {
         CliSpecDeriver.CliSpecInterfaceCheck<T>( out Type t, out CliDefinitionAttribute cliDefAttrib );

         T config = Config.New<T>( );
         config.Backend = new ParserResultConfigBackend( _specDeriverConfig, _spec, pr, t );
         config.Read( true );

         return config;
      }

   }

}
