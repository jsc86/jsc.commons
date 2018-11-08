// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Linq;
using System.Reflection;

using Castle.DynamicProxy;

using jsc.commons.config.interfaces;

namespace jsc.commons.config {

   internal class ConfigInterceptorSelector : IInterceptorSelector {

      public static ConfigInterceptorSelector Instance { get; } = new ConfigInterceptorSelector( );

      public IInterceptor[] SelectInterceptors( Type type, MethodInfo method, IInterceptor[] interceptors ) {
         ConfigBaseInterceptor cbi =
               (ConfigBaseInterceptor)interceptors.FirstOrDefault( i => i is ConfigBaseInterceptor );

         ConfigPropertyInterceptor cpi =
               (ConfigPropertyInterceptor)interceptors.FirstOrDefault( i => i is ConfigPropertyInterceptor );

         return typeof( IConfiguration ).GetMethods( ).Contains( method )
               ? new IInterceptor[] {
                     cbi
               }
               : new IInterceptor[] {
                     cpi
               };
      }

   }

}