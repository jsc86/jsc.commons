// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Castle.Core.Internal;
using Castle.DynamicProxy;

using jsc.commons.config.interfaces;

namespace jsc.commons.config {

   internal class ConfigPropertyInterceptor : IInterceptor {

      private static readonly Dictionary<string, Dictionary<string, Tuple<object, Type>>> __defaults;
      private static readonly Dictionary<string, Dictionary<string, PropertyInfo>> __propMaps;

      private readonly ConfigBase _cb;
      private readonly Dictionary<string, PropertyInfo> _propMap;

      static ConfigPropertyInterceptor( ) {
         __defaults = new Dictionary<string, Dictionary<string, Tuple<object, Type>>>( );
         __propMaps = new Dictionary<string, Dictionary<string, PropertyInfo>>( );
      }

      public ConfigPropertyInterceptor( Type t, ConfigBase cb ) {
         _cb = cb;
         _cb.Values = GetDefaults( t );
         _propMap = __propMaps[ t.FullName ];
      }

      public void Intercept( IInvocation invocation ) {
         MethodInfo mi = invocation.Method;
         PropertyInfo pi;
         if( !_propMap.TryGetValue( mi.Name, out pi ) )
            throw new ApplicationException( $"{mi.Name} is not handled by {nameof( ConfigPropertyInterceptor )}" );

         if( mi.Name.StartsWith( "get" ) )
            invocation.ReturnValue = _cb[ pi.Name ];
         else
            _cb[ pi.Name ] = invocation.Arguments[ 0 ];
      }

      private static Dictionary<string, Tuple<object, Type>> GetDefaults( Type t ) {
         string key = t.FullName;
         Dictionary<string, Tuple<object, Type>> defaults;
         if( __defaults.TryGetValue( key, out defaults ) )
            return CopyDefaults( t, defaults );

         ReadDefaults( t );
         return CopyDefaults( t );
      }

      private static Dictionary<string, Tuple<object, Type>> CopyDefaults(
            Type t,
            Dictionary<string, Tuple<object, Type>> defaults = null ) {
         defaults = defaults??__defaults[ t.FullName ];
         Dictionary<string, Tuple<object, Type>> copy = new Dictionary<string, Tuple<object, Type>>( defaults.Count );
         foreach( KeyValuePair<string, Tuple<object, Type>> kvp in defaults )
            copy[ kvp.Key ] =
                  new Tuple<object, Type>(
                        kvp.Value.Item1 is ICloneable
                              ? ( (ICloneable)kvp.Value.Item1 )?.Clone( )
                              : kvp.Value.Item1,
                        kvp.Value.Item2 );

         return copy;
      }

      private static void ReadDefaults( Type t ) {
         lock( __defaults ) {
            string key = t.FullName;
            if( __defaults.ContainsKey( key ) )
               return;

            IDefaultsProvider dp = GetDefaultsProvider( t );

            // read default values
            List<PropertyInfo> props = t.GetProperties( )
                  .Where( p => p.GetAttribute<ConfigValueAttribute>( ) != null )
                  .ToList( );
            Dictionary<string, Tuple<object, Type>> defaults =
                  new Dictionary<string, Tuple<object, Type>>( props.Count );
            props.ForEach(
                  pi =>
                        defaults[ pi.Name ] =
                              new Tuple<object, Type>(
                                    pi.GetAttribute<ConfigValueAttribute>( ).Default??dp?.GetDefault( pi.Name ),
                                    pi.PropertyType ) );
            __defaults[ key ] = defaults;

            // get property infos
            Dictionary<string, PropertyInfo> propMap = new Dictionary<string, PropertyInfo>( );
            props.ForEach(
                  pi => {
                     propMap[ pi.GetMethod.Name ] = pi;
                     propMap[ pi.SetMethod.Name ] = pi;
                  } );
            __propMaps[ key ] = propMap;
         }
      }

      private static IDefaultsProvider GetDefaultsProvider( Type t ) {
         return (IDefaultsProvider)t.GetAttribute<ConfigAttribute>( )
               ?.DefaultsProvider
               ?.GetConstructor( Type.EmptyTypes )
               ?.Invoke( new object[0] );
      }

   }

}