// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Reflection;

namespace jsc.commons.misc {

   public class Lazy<T> where T : class {

      // ReSharper disable once StaticMemberInGenericType
      private static readonly object[] __emptyObjArr = new object[0];
      private readonly Func<T> _spawner;

      private T _instance;

      public Lazy( ) {
         ConstructorInfo ctor = typeof( T ).GetConstructor( Type.EmptyTypes );
         _spawner = ( ) => (T)ctor?.Invoke( __emptyObjArr );
      }

      public Lazy( Func<T> spawner ) {
         _spawner = spawner;
      }

      public T Instance {
         get {
            if( _instance == null )
               lock( this ) {
                  if( _instance == null )
                     _instance = _spawner( );
               }

            return _instance;
         }
      }

      public bool IsInitialized => _instance != null;

   }

}