// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Concurrent;
using System.Reflection;

using jsc.commons.behaving.interfaces;
using jsc.commons.hierarchy.meta.interfaces;

namespace jsc.commons.hierarchy.meta {

   public static class MetaExtensions {

      private static readonly ConcurrentDictionary<Type, object> __blackList =
            new ConcurrentDictionary<Type, object>( );

      private static readonly ConcurrentDictionary<Type, IMetaAutoModHandler> __autoModHandlers =
            new ConcurrentDictionary<Type, IMetaAutoModHandler>( );

      private static readonly object __lockObj = new object( );

      public static bool TryGetAutoModHandler( this IBehavior meta, out IMetaAutoModHandler autoModHandler ) {
         autoModHandler = null;
         Type t = meta.GetType( );
         if( __blackList.ContainsKey( t ) )
            return false;

         if( __autoModHandlers.TryGetValue( t, out autoModHandler ) )
            return true;

         lock( __lockObj ) {
            if( __autoModHandlers.TryGetValue( t, out autoModHandler ) )
               return true;

            MetaAutoModHandlerAttribute attrib =
                  (MetaAutoModHandlerAttribute)t.GetCustomAttribute( typeof( MetaAutoModHandlerAttribute ) );

            if( attrib == null ) {
               __blackList[ t ] = null;
               return false;
            }

            Type autoModHandlerType = attrib.MetaAutoModHandlerType;
            autoModHandler = (IMetaAutoModHandler)Activator.CreateInstance( autoModHandlerType );

            __autoModHandlers[ t ] = autoModHandler;
         }

         return true;
      }

   }

}