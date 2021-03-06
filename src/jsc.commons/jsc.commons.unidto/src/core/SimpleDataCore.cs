// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.unidto.core.interfaces;

namespace jsc.commons.unidto.core {

   public class SimpleDataCore : IDirty {

      private readonly object[] _data;
      private readonly NotifyPropertyChanged _npc;
      private readonly PropertyMapper _propertyMapper;
      private bool _isDirty;

      public SimpleDataCore( Type type, NotifyPropertyChanged npc ) {
         if( type == null )
            throw new ArgumentNullException( nameof( type ), $"{nameof( type )} must not be null" );

         Type = type;
         _npc = npc;
         _propertyMapper = PropertyMapper.GetPropertyMapper( type );
         _data = new object[_propertyMapper.Count];
      }

      public bool IsNull( string key ) {
         return Get( key ) == null;
      }

      public object Get( string key ) {
         return _data[ _propertyMapper.GetPropertyIndex( key ) ];
      }

      public void Set( string key, object value ) {
         IsDirty = true;
         _data[ _propertyMapper.GetPropertyIndex( key ) ] = value;
      }

      public object[] GetData( ) {
         object[] dataCopy = new object[_data.Length];
         Array.Copy( _data, dataCopy, _data.Length );
         return dataCopy;
      }

      public Type Type { get; }

      public bool IsDirty {
         get => _isDirty;
         private set {
            if( value == _isDirty )
               return;
            _isDirty = value;
            _npc?.OnPropertyChanged( );
         }
      }

      public void MarkNotDirty( ) {
         IsDirty = false;
      }

   }

}