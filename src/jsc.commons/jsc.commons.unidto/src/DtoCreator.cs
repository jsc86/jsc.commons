// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Castle.DynamicProxy;
using Castle.DynamicProxy.Internal;

using jsc.commons.unidto.core;
using jsc.commons.unidto.core.attributes;
using jsc.commons.unidto.core.interfaces;
using jsc.commons.unidto.dynamic;

namespace jsc.commons.unidto {

   public static class DtoCreator {

      private static ProxyGenerator _pg;
      private static readonly misc.Lazy<ProxyGenerator> __lazyPg;
      private static readonly ProxyGenerationOptions __pgo;

      static DtoCreator( ) {
         __lazyPg = new misc.Lazy<ProxyGenerator>( );
         __pgo = new ProxyGenerationOptions {
               Selector = DtoInterceptorSelector.Instance
         };
      }

      public static ProxyGenerator ProxyGenerator {
         get => _pg??__lazyPg.Instance;
         set => _pg = value;
      }

      public static T New<T>( ) where T : class {
         IList<Type> facetTypes = GetFacetTypes<T>( );
         Type dataCoreType = facetTypes.First( t => t.GetInterfaces( ).Contains( typeof( IDataCore ) ) );
         facetTypes = facetTypes.Except( new[] {dataCoreType} ).ToList( );

         T instance = null;

         NotifyPropertyChanged npc = null;
         Type npcType = facetTypes.FirstOrDefault( t => t == typeof( INotifyPropertyChanged ) );
         if( npcType != null ) {
            // ReSharper disable once AccessToModifiedClosure
            npc = new NotifyPropertyChanged( ( ) => instance );
            facetTypes = facetTypes.Except( new[] {npcType} ).ToList( );
         }

         DataCoreInterceptor dci = new DataCoreInterceptor( dataCoreType, typeof( T ), npc );
         DtoInterceptor di = new DtoInterceptor( typeof( T ), dci.DataCore, npc );

         // todo: create interceptors for facets

         if( npc == null )
            instance = (T)ProxyGenerator.CreateInterfaceProxyWithoutTarget(
                  typeof( T ),
                  Array.Empty<Type>( ),
                  __pgo,
                  di,
                  dci );
         else
            instance = (T)ProxyGenerator.CreateInterfaceProxyWithoutTarget(
                  typeof( T ),
                  Array.Empty<Type>( ),
                  __pgo,
                  di,
                  dci,
                  new NotifyPropertyChangedInterceptor( npc ) );

         return instance;
      }

      private static IList<Type> GetFacetTypes<T>( ) {
         return typeof( T ).GetAllInterfaces( )
               .Where( it => it.GetCustomAttribute<MarkerAttribute>( ) != null )
               .ToList( );
      }

   }

}