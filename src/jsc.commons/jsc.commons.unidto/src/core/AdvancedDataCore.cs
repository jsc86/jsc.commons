// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.unidto.core.interfaces;

namespace jsc.commons.unidto.core {

   public class AdvancedDataCore : IChangeable {

      private readonly NotifyPropertyChanged _npc;

      private readonly PropertyMapper _propertyMapper;

      private object[] _currentData;
      private bool _hasChanges;
      private object[] _previousData;

      public AdvancedDataCore( Type type, NotifyPropertyChanged npc ) {
         if( type == null )
            throw new ArgumentNullException( nameof( type ), $"{nameof( type )} must not be null" );

         Type = type;
         _npc = npc;
         _propertyMapper = PropertyMapper.GetPropertyMapper( type );
         _currentData = new object[_propertyMapper.Count];
      }

      public bool HasChanges {
         get => _hasChanges;
         private set {
            if( value == _hasChanges )
               return;
            _hasChanges = value;
            _npc?.OnPropertyChanged( );
         }
      }

      public void AcceptChanges( ) {
         _previousData = null;
         CheckForChanges( );
      }

      public void RevertChanges( ) {
         if( _npc == null ) {
            RevertChangesInternal( );
            return;
         }

         object[] tempCurrentData = GetData( );
         if( _previousData == null ) {
            RevertChangesInternal( );
            NotifyDataPropertyChangedAfterRevert( tempCurrentData, null );
         } else {
            object[] tempPreviousData = new object[_currentData.Length];
            Array.Copy( _previousData, tempPreviousData, _currentData.Length );
            RevertChangesInternal( );
            NotifyDataPropertyChangedAfterRevert( tempCurrentData, tempPreviousData );
         }
      }

      public bool IsNull( string key ) {
         return Get( key ) == null;
      }

      public object Get( string key ) {
         return _currentData[ _propertyMapper.GetPropertyIndex( key ) ];
      }

      public void Set( string key, object value ) {
         if( _previousData == null ) {
            _previousData = new object[_currentData.Length];
            Array.Copy( _currentData, _previousData, _currentData.Length );
         }

         _currentData[ _propertyMapper.GetPropertyIndex( key ) ] = value;
         CheckForChanges( );
      }

      public object[] GetData( ) {
         object[] dataCopy = new object[_currentData.Length];
         Array.Copy( _currentData, dataCopy, _currentData.Length );
         return dataCopy;
      }

      public Type Type { get; }

      private void NotifyDataPropertyChangedAfterRevert( object[] tempCurrentData, object[] tempPreviousData ) {
         for( int i = 0,
               l = tempCurrentData.Length;
               i < l;
               i++ )
            if( tempPreviousData == null
                  ? tempCurrentData[ i ] != null
                  : !tempCurrentData[ i ]?.Equals( tempPreviousData[ i ] )??tempPreviousData[ i ] != null )
               _npc.OnPropertyChanged( _propertyMapper.GetPropertyName( i ) );
      }

      private void CheckForChanges( ) {
         if( _previousData == null ) {
            HasChanges = false;
            return;
         }

         for( int i = 0,
               l = _currentData.Length;
               i < l;
               i++ )
            if( _currentData[ i ] != _previousData[ i ] ) {
               HasChanges = true;
               return;
            }

         HasChanges = false;
      }

      private void RevertChangesInternal( ) {
         if( _previousData == null ) {
            _currentData = new object[_currentData.Length];
         } else {
            _currentData = _previousData;
            _previousData = null;
         }

         CheckForChanges( );
      }

   }

}