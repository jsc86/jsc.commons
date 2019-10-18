// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using jsc.commons.misc;
using jsc.commons.unidto.core.attributes;

namespace jsc.commons.unidto.core {

   public class PropertyMapper {

      private static readonly object __lockObj = new object( );

      private static readonly Dictionary<Type, PropertyMapper> __typeMapping =
            new Dictionary<Type, PropertyMapper>( );

      private Dictionary<int, string> _indexMapping;

      private Dictionary<string, int> _propertyMapping;

      private PropertyMapper( Type type ) {
         if( type == null )
            throw new ArgumentNullException( nameof( type ), $"{nameof( type )} must not be null" );

         Type = type;
         Init( );
      }

      public Type Type { get; }

      public int Count => _propertyMapping.Count;

      private void Init( ) {
         PropertyInfo[] propertyInfos = Type.SelectManyRecursiveInclusive( i => i.GetInterfaces( ) )
               .Where( i => i.GetCustomAttribute<MarkerAttribute>( ) == null )
               .SelectMany( i => i.GetProperties( BindingFlags.Instance|BindingFlags.Public ) )
               .ToArray( );
         _propertyMapping = new Dictionary<string, int>( propertyInfos.Length );
         _indexMapping = new Dictionary<int, string>( propertyInfos.Length );
         int index = 0;
         foreach( PropertyInfo propertyInfo in propertyInfos ) {
            _propertyMapping[ propertyInfo.Name ] = index;
            _indexMapping[ index ] = propertyInfo.Name;
            index++;
         }
      }

      public static PropertyMapper GetPropertyMapper( Type type ) {
         // ReSharper disable once InconsistentlySynchronizedField
         if( __typeMapping.TryGetValue( type, out PropertyMapper propertyMapper ) )
            return propertyMapper;

         lock( __lockObj ) {
            if( __typeMapping.TryGetValue( type, out propertyMapper ) )
               return propertyMapper;

            propertyMapper = new PropertyMapper( type );
            __typeMapping[ type ] = propertyMapper;
            return propertyMapper;
         }
      }

      public int GetPropertyIndex( string propertyName ) {
         return _propertyMapping[ propertyName ];
      }

      public string GetPropertyName( int propertyIndex ) {
         return _indexMapping[ propertyIndex ];
      }

   }

}