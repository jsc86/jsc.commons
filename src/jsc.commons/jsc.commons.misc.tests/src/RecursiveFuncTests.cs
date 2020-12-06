// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Linq;

using NUnit.Framework;

namespace jsc.commons.misc.tests {

   [TestFixture( typeof( RecursiveFunc<bool, int> ) )]
   [TestFixture( typeof( RecursiveFunc<bool, int, int> ) )]
   [TestFixture( typeof( RecursiveFunc<bool, int, int, int> ) )]
   [TestFixture( typeof( RecursiveFunc<bool, int, int, int, int> ) )]
   [TestFixture( typeof( RecursiveFunc<bool, int, int, int, int, int> ) )]
   [TestFixture( typeof( RecursiveFunc<bool, int, int, int, int, int, int> ) )]
   [TestFixture( typeof( RecursiveFunc<bool, int, int, int, int, int, int, int> ) )]
   [TestFixture( typeof( RecursiveFunc<bool, int, int, int, int, int, int, int, int> ) )]
   [TestFixture( typeof( RecursiveFunc<bool, int, int, int, int, int, int, int, int, int> ) )]
   [TestFixture( typeof( RecursiveFunc<bool, int, int, int, int, int, int, int, int, int, int> ) )]
   [TestFixture( typeof( RecursiveFunc<bool, int, int, int, int, int, int, int, int, int, int, int> ) )]
   [TestFixture( typeof( RecursiveFunc<bool, int, int, int, int, int, int, int, int, int, int, int, int> ) )]
   [TestFixture( typeof( RecursiveFunc<bool, int, int, int, int, int, int, int, int, int, int, int, int, int> ) )]
   [TestFixture( typeof( RecursiveFunc<bool, int, int, int, int, int, int, int, int, int, int, int, int, int, int> ) )]
   [TestFixture(
         typeof( RecursiveFunc<bool, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> ) )]
   [TestFixture(
         typeof( RecursiveFunc<bool, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int
         > ) )]
   public class RecursiveFuncTests<T> {

      private int Impl( T ra, bool firstCall ) {
         return GenericImpl( ra, firstCall );
      }

      private int Impl( T ra, bool firstCall, int arg1 ) {
         return GenericImpl( ra, firstCall, arg1 );
      }

      private int Impl( T ra, bool firstCall, int arg1, int arg2 ) {
         return GenericImpl( ra, firstCall, arg1, arg2 );
      }

      private int Impl( T ra, bool firstCall, int arg1, int arg2, int arg3 ) {
         return GenericImpl( ra, firstCall, arg1, arg2, arg3 );
      }

      private int Impl( T ra, bool firstCall, int arg1, int arg2, int arg3, int arg4 ) {
         return GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4 );
      }

      private int Impl( T ra, bool firstCall, int arg1, int arg2, int arg3, int arg4, int arg5 ) {
         return GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4, arg5 );
      }

      private int Impl( T ra, bool firstCall, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6 ) {
         return GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4, arg5, arg6 );
      }

      private int Impl( T ra, bool firstCall, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7 ) {
         return GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4, arg5, arg6, arg7 );
      }

      private int Impl(
            T ra,
            bool firstCall,
            int arg1,
            int arg2,
            int arg3,
            int arg4,
            int arg5,
            int arg6,
            int arg7,
            int arg8 ) {
         return GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 );
      }

      private int Impl(
            T ra,
            bool firstCall,
            int arg1,
            int arg2,
            int arg3,
            int arg4,
            int arg5,
            int arg6,
            int arg7,
            int arg8,
            int arg9 ) {
         return GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 );
      }

      private int Impl(
            T ra,
            bool firstCall,
            int arg1,
            int arg2,
            int arg3,
            int arg4,
            int arg5,
            int arg6,
            int arg7,
            int arg8,
            int arg9,
            int arg10 ) {
         return GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 );
      }

      private int Impl(
            T ra,
            bool firstCall,
            int arg1,
            int arg2,
            int arg3,
            int arg4,
            int arg5,
            int arg6,
            int arg7,
            int arg8,
            int arg9,
            int arg10,
            int arg11 ) {
         return GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 );
      }

      private int Impl(
            T ra,
            bool firstCall,
            int arg1,
            int arg2,
            int arg3,
            int arg4,
            int arg5,
            int arg6,
            int arg7,
            int arg8,
            int arg9,
            int arg10,
            int arg11,
            int arg12 ) {
         return GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 );
      }

      private int Impl(
            T ra,
            bool firstCall,
            int arg1,
            int arg2,
            int arg3,
            int arg4,
            int arg5,
            int arg6,
            int arg7,
            int arg8,
            int arg9,
            int arg10,
            int arg11,
            int arg12,
            int arg13 ) {
         return GenericImpl(
               ra,
               firstCall,
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

      private int Impl(
            T ra,
            bool firstCall,
            int arg1,
            int arg2,
            int arg3,
            int arg4,
            int arg5,
            int arg6,
            int arg7,
            int arg8,
            int arg9,
            int arg10,
            int arg11,
            int arg12,
            int arg13,
            int arg14 ) {
         return GenericImpl(
               ra,
               firstCall,
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

      private int Impl(
            T ra,
            bool firstCall,
            int arg1,
            int arg2,
            int arg3,
            int arg4,
            int arg5,
            int arg6,
            int arg7,
            int arg8,
            int arg9,
            int arg10,
            int arg11,
            int arg12,
            int arg13,
            int arg14,
            int arg15 ) {
         return GenericImpl(
               ra,
               firstCall,
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

      private int GenericImpl( T ra, bool firstCall, params int[] args ) {
         int sum = 0;
         for( int i = 0; i < args.Length; i++ )
            sum += args[ i ] = args[ i ]+1;

         if( firstCall ) {
            object[] newArgs = new object[args.Length+2];
            newArgs[ 0 ] = ra;
            newArgs[ 1 ] = false;
            Array.Copy( args, 0, newArgs, 2, args.Length );
            sum += (int)( (Delegate)(object)ra ).DynamicInvoke( newArgs );
         }

         return sum;
      }

      [Test]
      public void Test( ) {
         Delegate rf = Delegate.CreateDelegate( typeof( T ), this, nameof( Impl ) );
         object[] args = new object[typeof( T ).GenericTypeArguments.Length];
         args[ 0 ] = rf;
         args[ 1 ] = true;
         for( int i = 2,
               l = args.Length;
               i < l;
               i++ )
            args[ i ] = default( int );

         int n = (int)rf.DynamicInvoke( args );

         Assert.AreEqual( ( typeof( T ).GenericTypeArguments.Length-2 )*3, n );
      }

      [Test]
      public void TestEx( ) {
         Type[] rfTypeArgs = typeof( T ).GenericTypeArguments;
         Delegate rf = Delegate.CreateDelegate( typeof( T ), this, nameof( Impl ) );

         // ReSharper disable once PossibleNullReferenceException
         Delegate func = (Delegate)typeof( RecursiveFuncEx ).GetMethods( )
               .FirstOrDefault(
                     mi => mi.Name == nameof( RecursiveFuncEx.RF2F )
                           &&mi.GetGenericArguments( ).Length == rfTypeArgs.Length )
               .MakeGenericMethod( rfTypeArgs )
               .Invoke( null, new object[] {rf} );

         object[] args = new object[rfTypeArgs.Length-1];
         args[ 0 ] = true;
         for( int i = 1,
               l = args.Length;
               i < l;
               i++ )
            args[ i ] = default( int );

         int n = (int)func.DynamicInvoke( args );

         Assert.AreEqual( ( rfTypeArgs.Length-2 )*3, n );
      }

   }

}