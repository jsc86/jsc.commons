// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Reflection;

using Castle.DynamicProxy;

using jsc.commons.config.interfaces;

namespace jsc.commons.config {

   internal class ConfigBaseInterceptor : IInterceptor, IConfiguration {

      private static readonly string __save;
      private static readonly string __read;
      private static readonly string __backendGet;
      private static readonly string __backendSet;
      private static readonly string __keys;

      private readonly ConfigBase _cb;

      static ConfigBaseInterceptor( ) {
         __save = nameof( IConfiguration.Save );
         __read = nameof( IConfiguration.Read );
         PropertyInfo pi = typeof( IConfiguration ).GetProperty( nameof( IConfiguration.Backend ) );

         // ReSharper disable once PossibleNullReferenceException
         __backendGet = pi.GetMethod.Name;
         __backendSet = pi.SetMethod.Name;

         // ReSharper disable once PossibleNullReferenceException
         __keys = typeof( IConfiguration ).GetProperty( nameof( IConfiguration.Keys ) ).GetMethod.Name;
      }

      public ConfigBaseInterceptor( ConfigBase cb ) {
         _cb = cb;
      }

      public void Save( ) {
         _cb.Save( );
      }

      public void Read( bool skipNullValues = false ) {
         _cb.Read( skipNullValues );
      }

      public IConfigurationBackend Backend {
         get => _cb.Backend;
         set => _cb.Backend = value;
      }

      public object this[ string key, Type type = null ] {
         get => _cb[ key, type ];
         set => _cb[ key, type ] = value;
      }

      public Tuple<object, Type> GetConfigProperty( string key ) {
         return _cb.GetConfigProperty( key );
      }

      public IEnumerable<string> Keys => _cb.Keys;

      public void Intercept( IInvocation invocation ) {
         string mn = invocation.Method.Name;
         if( mn.Equals( __save ) )
            Save( );
         else if( mn.Equals( __read ) )
            Read( (bool)invocation.Arguments[ 0 ] );
         else if( mn.Equals( __keys ) )
            invocation.ReturnValue = Keys;
         else if( mn.Equals( __backendGet ) )
            invocation.ReturnValue = Backend;
         else if( mn.Equals( __backendSet ) )
            Backend = (IConfigurationBackend)invocation.Arguments[ 0 ];
         else
            throw new ApplicationException(
                  $"{invocation.Method.Name} is not implemented by {nameof( ConfigBaseInterceptor )}" );
      }

   }

}
