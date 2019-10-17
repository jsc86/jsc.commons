// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace jsc.commons.async {

   public static class ThreadPoolEx {

      public static Task InvokeAsync( Action action ) {
         TaskCompletionSource<object> tcl = new TaskCompletionSource<object>( );
         ThreadPool.QueueUserWorkItem(
               _ => {
                  try {
                     action( );
                     tcl.SetResult( null );
                  } catch( Exception exc ) {
                     tcl.SetException( exc );
                  }
               } );
         return tcl.Task;
      }

      public static Task InvokeAsync( Func<Task> func ) {
         TaskCompletionSource<object> tcl = new TaskCompletionSource<object>( );
         ThreadPool.QueueUserWorkItem(
               _ => {
                  Task rt = func( );
                  ConfiguredTaskAwaitable.ConfiguredTaskAwaiter cta =
                        rt.ConfigureAwait( false ).GetAwaiter( );
                  cta.OnCompleted(
                        ( ) => {
                           if( rt.IsCanceled )
                              tcl.SetCanceled( );
                           else if( rt.IsFaulted )
                                 // ReSharper disable once AssignNullToNotNullAttribute
                              tcl.SetException( rt.Exception );
                           else
                              tcl.SetResult( null );
                        } );
               } );
         return tcl.Task;
      }

      public static Task<T> InvokeAsync<T>( Func<T> func ) {
         TaskCompletionSource<T> tcl = new TaskCompletionSource<T>( );
         ThreadPool.QueueUserWorkItem(
               _ => {
                  try {
                     T result = func( );
                     tcl.SetResult( result );
                  } catch( Exception exc ) {
                     tcl.SetException( exc );
                  }
               } );
         return tcl.Task;
      }

      public static Task<T> InvokeAsync<T>( Func<Task<T>> func ) {
         TaskCompletionSource<T> tcl = new TaskCompletionSource<T>( );
         ThreadPool.QueueUserWorkItem(
               _ => {
                  try {
                     Task<T> rt = func( );
                     ConfiguredTaskAwaitable<T>.ConfiguredTaskAwaiter cta =
                           rt.ConfigureAwait( false ).GetAwaiter( );
                     cta.OnCompleted(
                           ( ) => {
                              if( rt.IsCanceled )
                                 tcl.SetCanceled( );
                              else if( rt.IsFaulted )
                                    // ReSharper disable once AssignNullToNotNullAttribute
                                 tcl.SetException( rt.Exception );
                              else
                                 tcl.SetResult( rt.Result );
                           } );
                  } catch( Exception exc ) {
                     tcl.SetException( exc );
                  }
               } );
         return tcl.Task;
      }

   }

}
