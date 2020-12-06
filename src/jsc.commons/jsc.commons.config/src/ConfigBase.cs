// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

using jsc.commons.config.interfaces;

namespace jsc.commons.config {

   internal class ConfigBase : IConfiguration {

      internal Dictionary<string, Tuple<object, Type>> Values { get; set; }

      public void Save( ) {
         Backend.Save( this );
      }

      public void Read( bool skipNullValues = false ) {
         Backend.Read( this, skipNullValues );
      }

      public IConfigurationBackend Backend { get; set; }

      public object this[ string key, Type type = null ] {
         get {
            Tuple<object, Type> configProp = GetConfigProperty( key );
            if( configProp == null )
               return null;

            if( configProp.Item2 != null
                  &&type != null
                  &&configProp.Item2 != type )
               throw new ApplicationException( $"config property type mismatch: {type}/{configProp.Item2}" );

            return configProp.Item1;
         }
         set {
            type ??= value?.GetType( );
            Tuple<object, Type> configProp = GetConfigProperty( key );
            if( configProp == null ) {
               if( type == null )
                  throw new ApplicationException( $"missing type information for new key '{key}'" );

               Values[ key ] = new Tuple<object, Type>( value, type );
            } else {
               if( type != null
                     &&type != configProp.Item2
                     &&!configProp.Item2.IsAssignableFrom( type ) )
                  throw new ApplicationException( $"type mismatch for key '{key}': {type}/{configProp.Item2}" );

               Values[ key ] = new Tuple<object, Type>( value, configProp.Item2 );
            }
         }
      }


      public Tuple<object, Type> GetConfigProperty( string key ) {
         return Values.TryGetValue( key, out Tuple<object, Type> configProp )? configProp : null;
      }

      public IEnumerable<string> Keys => Values.Keys;

   }

}