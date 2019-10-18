// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

namespace jsc.commons.misc {

   public delegate TResult RecursiveFunc<TResult>( RecursiveFunc<TResult> ra );

   public delegate TResult RecursiveFunc<T1, TResult>( RecursiveFunc<T1, TResult> ra, T1 arg1 );

   public delegate TResult RecursiveFunc<T1, T2, TResult>( RecursiveFunc<T1, T2, TResult> ra, T1 arg1, T2 arg2 );

   public delegate TResult RecursiveFunc<T1, T2, T3, TResult>(
         RecursiveFunc<T1, T2, T3, TResult> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3 );

   public delegate TResult RecursiveFunc<T1, T2, T3, T4, TResult>(
         RecursiveFunc<T1, T2, T3, T4, TResult> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4 );

   public delegate TResult RecursiveFunc<T1, T2, T3, T4, T5, TResult>(
         RecursiveFunc<T1, T2, T3, T4, T5, TResult> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4,
         T5 arg5 );

   public delegate TResult RecursiveFunc<T1, T2, T3, T4, T5, T6, TResult>(
         RecursiveFunc<T1, T2, T3, T4, T5, T6, TResult> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4,
         T5 arg5,
         T6 arg6 );

   public delegate TResult RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(
         RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, TResult> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4,
         T5 arg5,
         T6 arg6,
         T7 arg7 );

   public delegate TResult RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
         RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4,
         T5 arg5,
         T6 arg6,
         T7 arg7,
         T8 arg8 );

   public delegate TResult RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
         RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4,
         T5 arg5,
         T6 arg6,
         T7 arg7,
         T8 arg8,
         T9 arg9 );

   public delegate TResult RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
         RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4,
         T5 arg5,
         T6 arg6,
         T7 arg7,
         T8 arg8,
         T9 arg9,
         T10 arg10 );

   public delegate TResult RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
         RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4,
         T5 arg5,
         T6 arg6,
         T7 arg7,
         T8 arg8,
         T9 arg9,
         T10 arg10,
         T11 arg11 );

   public delegate TResult RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
         RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4,
         T5 arg5,
         T6 arg6,
         T7 arg7,
         T8 arg8,
         T9 arg9,
         T10 arg10,
         T11 arg11,
         T12 arg12 );

   public delegate TResult RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
         RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4,
         T5 arg5,
         T6 arg6,
         T7 arg7,
         T8 arg8,
         T9 arg9,
         T10 arg10,
         T11 arg11,
         T12 arg12,
         T13 arg13 );

   public delegate TResult RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
         RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4,
         T5 arg5,
         T6 arg6,
         T7 arg7,
         T8 arg8,
         T9 arg9,
         T10 arg10,
         T11 arg11,
         T12 arg12,
         T13 arg13,
         T14 arg14 );

   public delegate TResult RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
         RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4,
         T5 arg5,
         T6 arg6,
         T7 arg7,
         T8 arg8,
         T9 arg9,
         T10 arg10,
         T11 arg11,
         T12 arg12,
         T13 arg13,
         T14 arg14,
         T15 arg15 );

   public delegate TResult RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,
         TResult>(
         RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4,
         T5 arg5,
         T6 arg6,
         T7 arg7,
         T8 arg8,
         T9 arg9,
         T10 arg10,
         T11 arg11,
         T12 arg12,
         T13 arg13,
         T14 arg14,
         T15 arg15,
         T16 arg16 );

   public static class RecursiveFuncEx {

      public static Func<TResult> RF2F<TResult>( RecursiveFunc<TResult> rf ) {
         return ( ) => rf( rf );
      }

      public static Func<T1, TResult> RF2F<T1, TResult>( RecursiveFunc<T1, TResult> rf ) {
         return arg1 => rf( rf, arg1 );
      }

      public static Func<T1, T2, TResult> RF2F<T1, T2, TResult>( RecursiveFunc<T1, T2, TResult> rf ) {
         return ( arg1, arg2 ) => rf( rf, arg1, arg2 );
      }

      public static Func<T1, T2, T3, TResult> RF2F<T1, T2, T3, TResult>( RecursiveFunc<T1, T2, T3, TResult> rf ) {
         return ( arg1, arg2, arg3 ) => rf( rf, arg1, arg2, arg3 );
      }

      public static Func<T1, T2, T3, T4, TResult> RF2F<T1, T2, T3, T4, TResult>(
            RecursiveFunc<T1, T2, T3, T4, TResult> rf ) {
         return ( arg1, arg2, arg3, arg4 ) => rf( rf, arg1, arg2, arg3, arg4 );
      }

      public static Func<T1, T2, T3, T4, T5, TResult> RF2F<T1, T2, T3, T4, T5, TResult>(
            RecursiveFunc<T1, T2, T3, T4, T5, TResult> rf ) {
         return ( arg1, arg2, arg3, arg4, arg5 ) => rf( rf, arg1, arg2, arg3, arg4, arg5 );
      }

      public static Func<T1, T2, T3, T4, T5, T6, TResult> RF2F<T1, T2, T3, T4, T5, T6, TResult>(
            RecursiveFunc<T1, T2, T3, T4, T5, T6, TResult> rf ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6 ) => rf( rf, arg1, arg2, arg3, arg4, arg5, arg6 );
      }

      public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> RF2F<T1, T2, T3, T4, T5, T6, T7, TResult>(
            RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, TResult> rf ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7 ) => rf( rf, arg1, arg2, arg3, arg4, arg5, arg6, arg7 );
      }

      public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> RF2F<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> rf ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 ) => rf(
               rf,
               arg1,
               arg2,
               arg3,
               arg4,
               arg5,
               arg6,
               arg7,
               arg8 );
      }

      public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> RF2F<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> rf ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 ) => rf(
               rf,
               arg1,
               arg2,
               arg3,
               arg4,
               arg5,
               arg6,
               arg7,
               arg8,
               arg9 );
      }

      public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> RF2F<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            TResult>( RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> rf ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 ) => rf(
               rf,
               arg1,
               arg2,
               arg3,
               arg4,
               arg5,
               arg6,
               arg7,
               arg8,
               arg9,
               arg10 );
      }

      public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> RF2F<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, TResult>( RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> rf ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 ) => rf(
               rf,
               arg1,
               arg2,
               arg3,
               arg4,
               arg5,
               arg6,
               arg7,
               arg8,
               arg9,
               arg10,
               arg11 );
      }

      public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> RF2F<T1, T2, T3, T4, T5, T6, T7,
            T8, T9, T10, T11, T12, TResult>(
            RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> rf ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 ) => rf(
               rf,
               arg1,
               arg2,
               arg3,
               arg4,
               arg5,
               arg6,
               arg7,
               arg8,
               arg9,
               arg10,
               arg11,
               arg12 );
      }

      public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> RF2F<T1, T2, T3, T4, T5, T6,
            T7, T8, T9, T10, T11, T12, T13, TResult>(
            RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> rf ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 ) => rf(
               rf,
               arg1,
               arg2,
               arg3,
               arg4,
               arg5,
               arg6,
               arg7,
               arg8,
               arg9,
               arg10,
               arg11,
               arg12,
               arg13 );
      }

      public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> RF2F<T1, T2, T3, T4, T5,
            T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
            RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> rf ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 ) => rf(
               rf,
               arg1,
               arg2,
               arg3,
               arg4,
               arg5,
               arg6,
               arg7,
               arg8,
               arg9,
               arg10,
               arg11,
               arg12,
               arg13,
               arg14 );
      }

      public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> RF2F<T1, T2, T3, T4,
            T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
            RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> rf ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 ) =>
               rf(
                     rf,
                     arg1,
                     arg2,
                     arg3,
                     arg4,
                     arg5,
                     arg6,
                     arg7,
                     arg8,
                     arg9,
                     arg10,
                     arg11,
                     arg12,
                     arg13,
                     arg14,
                     arg15 );
      }


      public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> RF2F<T1, T2,
            T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
            RecursiveFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> rf ) {
         return (
                     arg1,
                     arg2,
                     arg3,
                     arg4,
                     arg5,
                     arg6,
                     arg7,
                     arg8,
                     arg9,
                     arg10,
                     arg11,
                     arg12,
                     arg13,
                     arg14,
                     arg15,
                     arg16 ) =>
               rf(
                     rf,
                     arg1,
                     arg2,
                     arg3,
                     arg4,
                     arg5,
                     arg6,
                     arg7,
                     arg8,
                     arg9,
                     arg10,
                     arg11,
                     arg12,
                     arg13,
                     arg14,
                     arg15,
                     arg16 );
      }

   }

}
