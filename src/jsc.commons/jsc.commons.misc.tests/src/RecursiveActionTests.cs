// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Linq;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace jsc.commons.misc.tests {

   [TestFixture( typeof( RecursiveAction<bool> ) )] //, TestName = "RecursiveAction<>1" )]
   [TestFixture(
         typeof( RecursiveAction<bool, object[]> ) )] //,
   //TestName = "RecursiveAction<>2" )]
   [TestFixture(
         typeof( RecursiveAction<bool, object[], object[]> ) )] //,
   //TestName = "RecursiveAction<>3" )]
   [TestFixture(
         typeof( RecursiveAction<bool, object[], object[], object[]> ) )] //,
   //TestName = "RecursiveAction<>4" )]
   [TestFixture(
         typeof( RecursiveAction<bool, object[], object[], object[], object[]> ) )] //,
   //TestName = "RecursiveAction<>5" )]
   [TestFixture(
         typeof( RecursiveAction<bool, object[], object[], object[], object[], object[]> ) )] //,
   //TestName = "RecursiveAction<>6" )]
   [TestFixture(
         typeof( RecursiveAction<bool, object[], object[], object[], object[], object[], object[]> ) )] //,
   //TestName = "RecursiveAction<>7" )]
   [TestFixture(
         typeof( RecursiveAction<bool, object[], object[], object[], object[], object[], object[], object[]> ) )] //,
   //TestName = "RecursiveAction<>8" )]
   [TestFixture(
         typeof( RecursiveAction<bool, object[], object[], object[], object[], object[], object[], object[], object[]
         > ) )] //,
   //TestName = "RecursiveAction<>9" )]
   [TestFixture(
         typeof( RecursiveAction<bool, object[], object[], object[], object[], object[], object[], object[], object[]
               , object[]> ) )] //,
   //TestName = "RecursiveAction<>10" )]
   [TestFixture(
         typeof( RecursiveAction<bool, object[], object[], object[], object[], object[], object[], object[], object[]
               , object[], object[]> ) )] //,
   //TestName = "RecursiveAction<>11" )]
   [TestFixture(
         typeof( RecursiveAction<bool, object[], object[], object[], object[], object[], object[], object[], object[]
               , object[], object[], object[]> ) )] //,
   //TestName = "RecursiveAction<>12" )]
   [TestFixture(
         typeof( RecursiveAction<bool, object[], object[], object[], object[], object[], object[], object[], object[]
               , object[], object[], object[], object[]> ) )] //,
   //TestName = "RecursiveAction<>13" )]
   [TestFixture(
         typeof( RecursiveAction<bool, object[], object[], object[], object[], object[], object[], object[], object[]
               , object[], object[], object[], object[], object[]> ) )] //,
   //TestName = "RecursiveAction<>14" )]
   [TestFixture(
         typeof( RecursiveAction<bool, object[], object[], object[], object[], object[], object[], object[], object[]
               , object[], object[], object[], object[], object[], object[]> ) )] //,
   //TestName = "RecursiveAction<>15" )]
   [TestFixture(
         typeof( RecursiveAction<bool, object[], object[], object[], object[], object[], object[], object[], object[]
               , object[], object[], object[], object[], object[], object[], object[]> ) )] //,
   //TestName = "RecursiveAction<>16" )]
   public class RecursiveActionTests<T> where T : class {

      private void Impl( T ra, bool firstCall ) {
         GenericImpl( ra, firstCall );
      }

      private void Impl( T ra, bool firstCall, object[] arg1 ) {
         GenericImpl( ra, firstCall, new object[] {arg1} );
      }

      private void Impl( T ra, bool firstCall, object[] arg1, object[] arg2 ) {
         GenericImpl( ra, firstCall, arg1, arg2 );
      }

      private void Impl( T ra, bool firstCall, object[] arg1, object[] arg2, object[] arg3 ) {
         GenericImpl( ra, firstCall, arg1, arg2, arg3 );
      }

      private void Impl( T ra, bool firstCall, object[] arg1, object[] arg2, object[] arg3, object[] arg4 ) {
         GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4 );
      }

      private void Impl(
            T ra,
            bool firstCall,
            object[] arg1,
            object[] arg2,
            object[] arg3,
            object[] arg4,
            object[] arg5 ) {
         GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4, arg5 );
      }

      private void Impl(
            T ra,
            bool firstCall,
            object[] arg1,
            object[] arg2,
            object[] arg3,
            object[] arg4,
            object[] arg5,
            object[] arg6 ) {
         GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4, arg5, arg6 );
      }


      private void Impl(
            T ra,
            bool firstCall,
            object[] arg1,
            object[] arg2,
            object[] arg3,
            object[] arg4,
            object[] arg5,
            object[] arg6,
            object[] arg7 ) {
         GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4, arg5, arg6, arg7 );
      }

      private void Impl(
            T ra,
            bool firstCall,
            object[] arg1,
            object[] arg2,
            object[] arg3,
            object[] arg4,
            object[] arg5,
            object[] arg6,
            object[] arg7,
            object[] arg8 ) {
         GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 );
      }

      private void Impl(
            T ra,
            bool firstCall,
            object[] arg1,
            object[] arg2,
            object[] arg3,
            object[] arg4,
            object[] arg5,
            object[] arg6,
            object[] arg7,
            object[] arg8,
            object[] arg9 ) {
         GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 );
      }

      private void Impl(
            T ra,
            bool firstCall,
            object[] arg1,
            object[] arg2,
            object[] arg3,
            object[] arg4,
            object[] arg5,
            object[] arg6,
            object[] arg7,
            object[] arg8,
            object[] arg9,
            object[] arg10 ) {
         GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 );
      }

      private void Impl(
            T ra,
            bool firstCall,
            object[] arg1,
            object[] arg2,
            object[] arg3,
            object[] arg4,
            object[] arg5,
            object[] arg6,
            object[] arg7,
            object[] arg8,
            object[] arg9,
            object[] arg10,
            object[] arg11 ) {
         GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11 );
      }

      private void Impl(
            T ra,
            bool firstCall,
            object[] arg1,
            object[] arg2,
            object[] arg3,
            object[] arg4,
            object[] arg5,
            object[] arg6,
            object[] arg7,
            object[] arg8,
            object[] arg9,
            object[] arg10,
            object[] arg11,
            object[] arg12 ) {
         GenericImpl( ra, firstCall, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12 );
      }

      private void Impl(
            T ra,
            bool firstCall,
            object[] arg1,
            object[] arg2,
            object[] arg3,
            object[] arg4,
            object[] arg5,
            object[] arg6,
            object[] arg7,
            object[] arg8,
            object[] arg9,
            object[] arg10,
            object[] arg11,
            object[] arg12,
            object[] arg13 ) {
         GenericImpl(
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

      private void Impl(
            T ra,
            bool firstCall,
            object[] arg1,
            object[] arg2,
            object[] arg3,
            object[] arg4,
            object[] arg5,
            object[] arg6,
            object[] arg7,
            object[] arg8,
            object[] arg9,
            object[] arg10,
            object[] arg11,
            object[] arg12,
            object[] arg13,
            object[] arg14 ) {
         GenericImpl(
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

      private void Impl(
            T ra,
            bool firstCall,
            object[] arg1,
            object[] arg2,
            object[] arg3,
            object[] arg4,
            object[] arg5,
            object[] arg6,
            object[] arg7,
            object[] arg8,
            object[] arg9,
            object[] arg10,
            object[] arg11,
            object[] arg12,
            object[] arg13,
            object[] arg14,
            object[] arg15 ) {
         GenericImpl(
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

      private void GenericImpl( T ra, bool firstCall, params object[] args ) {
         for( int i = 0,
               l = args.Length;
               i < l;
               i++ )
            if( firstCall )
               ( (object[])args[ i ] )[ 0 ] = i;
            else
               ( (object[])args[ i ] )[ 0 ] = (int)( (object[])args[ i ] )[ 0 ]+1;

         if( firstCall ) {
            object[] newArgs = new object[args.Length+2];
            newArgs[ 0 ] = ra;
            newArgs[ 1 ] = false;
            Array.Copy( args, 0, newArgs, 2, args.Length );
            ( (Delegate)(object)ra ).DynamicInvoke( newArgs );
         }
      }

      [Test]
      public void Test( ) {
         Delegate ra = Delegate.CreateDelegate( typeof( T ), this, nameof( Impl ) );
         object[] args = new object[typeof( T ).GenericTypeArguments.Length+1];
         args[ 0 ] = ra;
         args[ 1 ] = true;
         for( int i = 2,
               l = args.Length;
               i < l;
               i++ )
            args[ i ] = new object[1];

         ra.DynamicInvoke( args );

         for( int i = 2,
               l = args.Length;
               i < l;
               i++ )
            ClassicAssert.AreEqual( i-1, ( (object[])args[ i ] )[ 0 ] );
      }

      [Test]
      public void TestEx( ) {
         Type[] raTypeArgs = typeof( T ).GenericTypeArguments;
         Delegate ra = Delegate.CreateDelegate( typeof( T ), this, nameof( Impl ) );

         // ReSharper disable once PossibleNullReferenceException
         Delegate action = (Delegate)typeof( RecursiveActionEx ).GetMethods( )
               .FirstOrDefault(
                     mi => mi.Name == nameof( RecursiveActionEx.RA2A )
                           &&mi.GetGenericArguments( ).Length == raTypeArgs.Length )
               .MakeGenericMethod( raTypeArgs )
               .Invoke( null, new object[] {ra} );

         object[] args = new object[typeof( T ).GenericTypeArguments.Length];
         args[ 0 ] = true;
         for( int i = 1,
               l = args.Length;
               i < l;
               i++ )
            args[ i ] = new object[1];

         action.DynamicInvoke( args );

         for( int i = 2,
               l = args.Length;
               i < l;
               i++ )
            ClassicAssert.AreEqual( i, ( (object[])args[ i ] )[ 0 ] );
      }

   }

}
