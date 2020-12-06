// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

using jsc.commons.config.interfaces;

namespace jsc.commons.config.backend {

   public abstract class ConfigBackendBase : IConfigurationBackend {

      public void Save( IConfiguration config ) {
         BeforeSave( );
         try {
            foreach( string key in config.Keys ) {
               Tuple<object, Type> configProp = config.GetConfigProperty( key );
               Save( key, configProp.Item2, configProp.Item1 );
            }
         } catch( Exception exc ) {
            OnSaveException( exc );
         } finally {
            AfterSave( );
         }
      }

      public void Read( IConfiguration config, bool skipNullValues = false ) {
         BeforeRead( );
         try {
            Dictionary<string, Tuple<object, Type>> read =
                  new Dictionary<string, Tuple<object, Type>>( );
            foreach( string key in config.Keys ) {
               Tuple<object, Type> configProp = config.GetConfigProperty( key );
               if( Read( key, configProp.Item2, out object value ) )
                  read[ key ] = new Tuple<object, Type>( value, configProp.Item2 );
            }

            foreach( KeyValuePair<string, Tuple<object, Type>> kvp in read )
               if( !skipNullValues
                     ||kvp.Value.Item1 != null )
                  config[ kvp.Key, kvp.Value.Item2 ] = kvp.Value.Item1;
         } catch( Exception exc ) {
            OnReadException( exc );
         } finally {
            AfterRead( );
         }
      }

      public T Read<T>( ) where T : IConfiguration {
         T config = Config.New<T>( );
         Read( config );
         return config;
      }

      /// <summary>
      ///    Read a value for the given key from the Configuration Backend.
      ///    This method should not throw an exception if no value is present for
      ///    a given key but rather return false.
      /// </summary>
      /// <param name="key"></param>
      /// <param name="type"></param>
      /// <param name="value"></param>
      /// <returns></returns>
      protected abstract bool Read( string key, Type type, out object value );

      /// <summary>
      ///    Save the given value for the given key in the Configuration Backend.
      /// </summary>
      /// <param name="key"></param>
      /// <param name="type"></param>
      /// <param name="value"></param>
      protected abstract void Save( string key, Type type, object value );

      /// <summary>
      ///    Build up code (i.e. open a file, DB connection etc) before reading
      ///    configuration values.
      /// </summary>
      protected abstract void BeforeRead( );

      /// <summary>
      ///    Tear down code (i.e. close a file, DB connection etc) after reading
      ///    configuration values.
      /// </summary>
      protected abstract void AfterRead( );

      /// <summary>
      ///    Called if a exception occurs in a call of Read.
      /// </summary>
      /// <param name="exc"></param>
      protected abstract void OnReadException( Exception exc );

      /// <summary>
      ///    Build up code (i.e. open a file, DB connection etc) before writing
      ///    configuration values.
      /// </summary>
      protected abstract void BeforeSave( );

      /// <summary>
      ///    Tear down code (i.e. close a file, DB connection etc) after writing
      ///    configuration values.
      /// </summary>
      protected abstract void AfterSave( );

      /// <summary>
      ///    Called if a exception occurs in a call of Save.
      /// </summary>
      /// <param name="exc"></param>
      protected abstract void OnSaveException( Exception exc );

   }

}