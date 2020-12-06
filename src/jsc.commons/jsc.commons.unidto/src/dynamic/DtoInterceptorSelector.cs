// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using Castle.DynamicProxy;

using jsc.commons.misc;
using jsc.commons.unidto.core.attributes;

namespace jsc.commons.unidto.dynamic {

   public class DtoInterceptorSelector : IInterceptorSelector {

      private DtoInterceptorSelector( ) { }

      public static DtoInterceptorSelector Instance { get; } = new DtoInterceptorSelector( );

      public IInterceptor[] SelectInterceptors( Type type, MethodInfo method, IInterceptor[] interceptors ) {
         DataCoreInterceptor dci = (DataCoreInterceptor)interceptors.FirstOrDefault( i => i is DataCoreInterceptor );
         DtoInterceptor di = (DtoInterceptor)interceptors.FirstOrDefault( i => i is DtoInterceptor );
         NotifyPropertyChangedInterceptor npci =
               (NotifyPropertyChangedInterceptor)interceptors.FirstOrDefault(
                     i => i is NotifyPropertyChangedInterceptor );

         // todo handle facets

         if( method.DeclaringType == dci.DataCoreType )
            return new IInterceptor[] {dci};

         if( method.DeclaringType == di.DtoType )
            return new IInterceptor[] {di};

         if( method.DeclaringType == typeof( INotifyPropertyChanged ) )
            return new IInterceptor[] {npci};

         if( di.DtoType.SelectManyRecursive( i => i.GetInterfaces( ) )
               .Where( i => i.GetCustomAttribute<MarkerAttribute>( ) == null )
               .Any( i => i == method.DeclaringType ) )
            return new IInterceptor[] {di};

         throw new Exception( $"{method.Name} is not handled by any interceptor" );
      }

   }

}