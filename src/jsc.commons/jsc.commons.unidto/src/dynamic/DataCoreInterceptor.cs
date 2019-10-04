// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Reflection;

using Castle.DynamicProxy;

using jsc.commons.unidto.core.attributes;
using jsc.commons.unidto.core.interfaces;

namespace jsc.commons.unidto.dynamic {

   public class DataCoreInterceptor : IInterceptor {

      public DataCoreInterceptor( Type dataCoreType, Type dtoType ) {
         DataCoreType = dataCoreType;
         Type implType = dataCoreType.GetCustomAttribute<ImplementationAttribute>( )?.Type;

         ConstructorInfo ci = implType.GetConstructor(
               BindingFlags.Instance|BindingFlags.Public,
               null,
               new[] {typeof( Type )},
               null );

         DataCore = (IDataCore)ci.Invoke( new object[] {dtoType} );
      }

      public IDataCore DataCore { get; }

      public Type DataCoreType { get; }

      public void Intercept( IInvocation invocation ) {
         invocation.ReturnValue = invocation.Method.Invoke( DataCore, invocation.Arguments );
      }

   }

}