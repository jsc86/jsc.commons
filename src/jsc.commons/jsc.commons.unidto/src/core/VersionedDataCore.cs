// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Linq;

using jsc.commons.unidto.core.interfaces;

namespace jsc.commons.unidto.core {

   public class VersionedDataCore : IVersioned {

      private readonly NotifyPropertyChanged _npc;

      private readonly PropertyMapper _propertyMapper;
      private object[] _current;
      private uint _currentVersion;
      private Change _firstChange;
      private bool _hasChanges;
      private Change _lastChange;

      private object[] _squashed;
      private uint _squashedVersion;

      public VersionedDataCore( Type type, NotifyPropertyChanged npc ) {
         if( type == null )
            throw new ArgumentNullException( nameof( type ), $"{nameof( type )} must not be null" );

         Type = type;
         _npc = npc;
         _propertyMapper = PropertyMapper.GetPropertyMapper( type );
         _current = new object[_propertyMapper.Count];
      }

      public bool IsNull( string key ) {
         return Get( key ) == null;
      }

      public object Get( string key ) {
         return _current[ _propertyMapper.GetPropertyIndex( key ) ];
      }

      public void Set( string key, object value ) {
         int index = _propertyMapper.GetPropertyIndex( key );
         _current[ index ] = value;
         CurrentVersion++;
         if( _firstChange == null ) {
            _lastChange = _firstChange = new Change( value, index );
         } else {
            Change change = new Change( value, index );
            _lastChange = _lastChange.Next = change;
         }

         CheckForChanges( );
      }

      public object[] GetData( ) {
         object[] dataCopy = new object[_current.Length];
         Array.Copy( _current, dataCopy, _current.Length );
         return dataCopy;
      }

      public Type Type { get; }

      public uint CurrentVersion {
         get => _currentVersion;
         private set {
            if( value == _currentVersion )
               return;
            _currentVersion = value;
            _npc?.OnPropertyChanged( );
         }
      }

      public void ResetToVersion( uint version ) {
         if( _npc == null ) {
            ResetToVersionInternal( version );
            return;
         }

         object[] tempCurrent = GetData( );
         ResetToVersionInternal( version );
         NotifyDataPropertyChangedAfterReset( tempCurrent );
      }

      public void ResetToSquashed( ) {
         if( _npc == null ) {
            ResetToSquashedInternal( );
            return;
         }

         object[] tempCurrent = GetData( );
         ResetToSquashedInternal( );
         NotifyDataPropertyChangedAfterReset( tempCurrent );
      }

      public void SquashChanges( ) {
         if( _squashed == null )
            _squashed = new object[_current.Length];

         Array.Copy( _current, _squashed, _current.Length );
         _firstChange = null;
         _lastChange = null;
         _squashedVersion = CurrentVersion;
         CheckForChanges( );
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

      private void NotifyDataPropertyChangedAfterReset( object[] previousCurrent ) {
         for( int i = 0,
               l = _current.Length;
               i < l;
               i++ )
            if( _squashed == null
                  ? previousCurrent[ i ] != null
                  : !previousCurrent[ i ]?.Equals( _squashed[ i ] )??_squashed[ i ] != null )
               _npc.OnPropertyChanged( _propertyMapper.GetPropertyName( i ) );
      }

      private void ResetToVersionInternal( uint version ) {
         if( _squashed != null )
            Array.Copy( _squashed, _current, _current.Length );
         else
            _current = new object[_current.Length];

         Change currentChange = _firstChange;
         _current[ currentChange.Index ] = currentChange.Value;
         for( uint i = _squashedVersion+1; i < version; i++ ) {
            currentChange = currentChange.Next;
            _current[ currentChange.Index ] = currentChange.Value;
         }

         CurrentVersion = version;
         _lastChange = currentChange;
         CheckForChanges( );
      }

      private void ResetToSquashedInternal( ) {
         if( _squashed == null )
            _current = new object[_current.Length];
         else
            Array.Copy( _squashed, _current, _current.Length );
         _firstChange = null;
         CurrentVersion = _squashedVersion;
         CheckForChanges( );
      }

      private void CheckForChanges( ) {
         if( _squashed == null ) {
            HasChanges = _current.Any( value => value != null );
            return;
         }

         for( int i = 0,
               l = _current.Length;
               i < l;
               i++ )
            if( _current[ i ] != _squashed[ i ] ) {
               HasChanges = true;
               return;
            }

         HasChanges = false;
      }

      private class Change {

         public readonly int Index;

         public readonly object Value;
         public Change Next;

         public Change( object value, int index ) {
            Value = value;
            Index = index;
         }

      }

   }

}
