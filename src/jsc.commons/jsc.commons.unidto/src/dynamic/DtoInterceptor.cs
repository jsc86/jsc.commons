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

using jsc.commons.misc;
using jsc.commons.unidto.core.attributes;
using jsc.commons.unidto.core.interfaces;

namespace jsc.commons.unidto.dynamic {

   public class DtoInterceptor : IInterceptor {

      private static readonly Dictionary<Type, Tuple<Dictionary<MethodInfo, string>, Dictionary<MethodInfo, string>>>
            __typeMappings =
                  new Dictionary<Type, Tuple<Dictionary<MethodInfo, string>, Dictionary<MethodInfo, string>>>( );

      private readonly IDataCore _dataCore;

      private Dictionary<MethodInfo, string> _getMethodsMapping;
      private Dictionary<MethodInfo, string> _setMethodsMapping;

      public DtoInterceptor( Type dtoType, IDataCore dataCore ) {
         if( dataCore == null )
            throw new ArgumentNullException( nameof( dataCore ), $"{nameof( dataCore )} must not be null" );
         if( dtoType == null )
            throw new ArgumentNullException( nameof( dtoType ), $"{nameof( dtoType )} must not be null" );

         _dataCore = dataCore;
         DtoType = dtoType;
         Init( );
      }

      public Type DtoType { get; }

      public void Intercept( IInvocation invocation ) {
         if( _getMethodsMapping.TryGetValue( invocation.Method, out string propName ) )
            invocation.ReturnValue = _dataCore.Get( propName );
         else if( _setMethodsMapping.TryGetValue( invocation.Method, out propName ) )
            _dataCore.Set( propName, invocation.Arguments[ 0 ] );
         else
            throw new Exception( $"{invocation.Method.Name} is not a DTO property" );
      }

      private void Init( ) {
         if( !__typeMappings.TryGetValue(
               _dataCore.Type,
               out Tuple<Dictionary<MethodInfo, string>, Dictionary<MethodInfo, string>> methodMapping ) ) {
            methodMapping = MapMethods( );
            __typeMappings[ _dataCore.Type ] = methodMapping;
         }

         _getMethodsMapping = methodMapping.Item1;
         _setMethodsMapping = methodMapping.Item2;
      }

      private Tuple<Dictionary<MethodInfo, string>, Dictionary<MethodInfo, string>> MapMethods( ) {
         //PropertyInfo[] pis = _dataCore.Type.GetProperties( BindingFlags.Instance|BindingFlags.Public );
         PropertyInfo[] pis = _dataCore.Type.GetInterfaces( )
               .SelectManyRecursiveInclusive( i => i.GetInterfaces( ) )
               .Where( i => i.GetCustomAttribute<MarkerAttribute>( ) == null )
               .SelectMany( i => i.GetProperties( ) )
               .ToArray( );

         Dictionary<MethodInfo, string> getMethodsMapping = new Dictionary<MethodInfo, string>( pis.Length );
         Dictionary<MethodInfo, string> setMethodsMapping = new Dictionary<MethodInfo, string>( pis.Length );

         foreach( PropertyInfo pi in pis ) {
            getMethodsMapping[ pi.GetMethod ] = pi.Name;
            if( pi.SetMethod != null )
               setMethodsMapping[ pi.SetMethod ] = pi.Name;
         }

         return new Tuple<Dictionary<MethodInfo, string>, Dictionary<MethodInfo, string>>(
               getMethodsMapping,
               setMethodsMapping );
      }

   }

}