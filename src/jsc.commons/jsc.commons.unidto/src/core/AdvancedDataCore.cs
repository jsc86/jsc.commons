// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.unidto.core.interfaces;

namespace jsc.commons.unidto.core {

   public class AdvancedDataCore : IChangeable {

      private readonly PropertyMapper _propertyMapper;

      private object[] _currentData;
      private object[] _previousData;

      public AdvancedDataCore( Type type ) {
         if( type == null )
            throw new ArgumentNullException( nameof( type ), $"{nameof( type )} must not be null" );

         Type = type;
         _propertyMapper = PropertyMapper.GetPropertyMapper( type );
         _currentData = new object[_propertyMapper.Count];
      }

      public bool HasChanges {
         get {
            if( _previousData == null )
               return false;

            for( int i = 0,
                  l = _currentData.Length;
                  i < l;
                  i++ )
               if( _currentData[ i ] != _previousData[ i ] )
                  return true;

            return false;
         }
      }

      public void AcceptChanges( ) {
         _previousData = null;
      }

      public void RevertChanges( ) {
         _currentData = _previousData;
         _previousData = null;
      }

      public bool IsNull( string key ) {
         return Get( key ) == null;
      }

      public object Get( string key ) {
         return _currentData[ _propertyMapper.GetIndex( key ) ];
      }

      public void Set( string key, object value ) {
         if( _previousData == null ) {
            _previousData = new object[_currentData.Length];
            Array.Copy( _currentData, _previousData, _currentData.Length );
         }

         _currentData[ _propertyMapper.GetIndex( key ) ] = value;
      }

      public object[] GetData( ) {
         object[] dataCopy = new object[_currentData.Length];
         Array.Copy( _currentData, dataCopy, _currentData.Length );
         return dataCopy;
      }

      public Type Type { get; }

   }

}
