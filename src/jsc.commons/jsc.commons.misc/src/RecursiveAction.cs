// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

namespace jsc.commons.misc {

   public delegate void RecursiveAction( RecursiveAction ra );

   public delegate void RecursiveAction<T>( RecursiveAction<T> ra, T arg );

   public delegate void RecursiveAction<T1, T2>( RecursiveAction<T1, T2> ra, T1 arg1, T2 arg2 );

   public delegate void RecursiveAction<T1, T2, T3>( RecursiveAction<T1, T2, T3> ra, T1 arg1, T2 arg2, T3 arg3 );

   public delegate void RecursiveAction<T1, T2, T3, T4>(
         RecursiveAction<T1, T2, T3, T4> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4 );

   public delegate void RecursiveAction<T1, T2, T3, T4, T5>(
         RecursiveAction<T1, T2, T3, T4, T5> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4,
         T5 arg5 );

   public delegate void RecursiveAction<T1, T2, T3, T4, T5, T6>(
         RecursiveAction<T1, T2, T3, T4, T5, T6> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4,
         T5 arg5,
         T6 arg6 );

   public delegate void RecursiveAction<T1, T2, T3, T4, T5, T6, T7>(
         RecursiveAction<T1, T2, T3, T4, T5, T6, T7> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4,
         T5 arg5,
         T6 arg6,
         T7 arg7 );

   public delegate void RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8>(
         RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4,
         T5 arg5,
         T6 arg6,
         T7 arg7,
         T8 arg8 );

   public delegate void RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
         RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> ra,
         T1 arg1,
         T2 arg2,
         T3 arg3,
         T4 arg4,
         T5 arg5,
         T6 arg6,
         T7 arg7,
         T8 arg8,
         T9 arg9 );

   public delegate void RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
         RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ra,
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

   public delegate void RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
         RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ra,
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

   public delegate void RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
         RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ra,
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

   public delegate void RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
         RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ra,
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

   public delegate void RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
         RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ra,
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

   public delegate void RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
         RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ra,
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

   public delegate void RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
         RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ra,
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


   public static class RecursiveActionEx {

      public static Action RA2A( RecursiveAction ra ) {
         return ( ) => ra( ra );
      }

      public static Action<T> RA2A<T>( RecursiveAction<T> ra ) {
         return arg => ra( ra, arg );
      }

      public static Action<T1, T2> RA2A<T1, T2>( RecursiveAction<T1, T2> ra ) {
         return ( arg1, arg2 ) => ra( ra, arg1, arg2 );
      }

      public static Action<T1, T2, T3> RA2A<T1, T2, T3>( RecursiveAction<T1, T2, T3> ra ) {
         return ( arg1, arg2, arg3 ) => ra( ra, arg1, arg2, arg3 );
      }

      public static Action<T1, T2, T3, T4> RA2A<T1, T2, T3, T4>( RecursiveAction<T1, T2, T3, T4> ra ) {
         return ( arg1, arg2, arg3, arg4 ) => ra( ra, arg1, arg2, arg3, arg4 );
      }

      public static Action<T1, T2, T3, T4, T5> RA2A<T1, T2, T3, T4, T5>( RecursiveAction<T1, T2, T3, T4, T5> ra ) {
         return ( arg1, arg2, arg3, arg4, arg5 ) => ra( ra, arg1, arg2, arg3, arg4, arg5 );
      }

      public static Action<T1, T2, T3, T4, T5, T6> RA2A<T1, T2, T3, T4, T5, T6>(
            RecursiveAction<T1, T2, T3, T4, T5, T6> ra ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6 ) => ra( ra, arg1, arg2, arg3, arg4, arg5, arg6 );
      }

      public static Action<T1, T2, T3, T4, T5, T6, T7> RA2A<T1, T2, T3, T4, T5, T6, T7>(
            RecursiveAction<T1, T2, T3, T4, T5, T6, T7> ra ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7 ) => ra( ra, arg1, arg2, arg3, arg4, arg5, arg6, arg7 );
      }

      public static Action<T1, T2, T3, T4, T5, T6, T7, T8> RA2A<T1, T2, T3, T4, T5, T6, T7, T8>(
            RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8> ra ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 ) => ra(
               ra,
               arg1,
               arg2,
               arg3,
               arg4,
               arg5,
               arg6,
               arg7,
               arg8 );
      }

      public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> RA2A<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> ra ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 ) => ra(
               ra,
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

      public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> RA2A<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ra ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 ) => ra(
               ra,
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

      public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> RA2A<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11>(
            RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ra ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 ) => ra(
               ra,
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

      public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> RA2A<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12>(
            RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ra ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 ) => ra(
               ra,
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

      public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> RA2A<T1, T2, T3, T4, T5, T6, T7, T8,
            T9, T10, T11, T12, T13>(
            RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ra ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13 ) => ra(
               ra,
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

      public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> RA2A<T1, T2, T3, T4, T5, T6, T7,
            T8, T9, T10, T11, T12, T13, T14>(
            RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ra ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14 ) => ra(
               ra,
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

      public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> RA2A<T1, T2, T3, T4, T5,
            T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ra ) {
         return ( arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15 ) =>
               ra(
                     ra,
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

      public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> RA2A<T1, T2, T3, T4,
            T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            RecursiveAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ra ) {
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
               ra(
                     ra,
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
