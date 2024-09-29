// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace jsc.commons.async.tests {

   [TestFixture]
   public class ExecutionTokenSpoolTests {

      private class FromToSpan {

         public DateTime From { get; set; }
         public DateTime To { get; set; }

         public bool Intersects( FromToSpan other ) {
            if( To > other.To
                  &&From < other.From )
               return true;
            if( To > other.From
                  &&To < other.From )
               return true;
            if( From < other.To
                  &&From > other.From )
               return true;
            return false;
         }

      }

      [Test]
      public void ExceptionOnDisposedSpool( ) {
         ExecutionTokenSpool ets = new ExecutionTokenSpool( );

         Task[] tasks = new Task[2];
         TaskCompletionSource<object> tcs1 = new TaskCompletionSource<object>( );
         TaskCompletionSource<object> tcs2 = new TaskCompletionSource<object>( );
         Task[] syncTasks = {tcs1.Task, tcs2.Task};
         AutoResetEvent resetEvent = new AutoResetEvent( false );

         tasks[ 0 ] =
               Task.Run(
                     async ( ) => {
                        tcs1.SetResult( null );
                        using( await ets.GetExecutionToken( ) ) {
                           resetEvent.WaitOne( );
                        }
                     } );
         tasks[ 1 ] =
               Task.Run(
                     async ( ) => {
                        tcs2.SetResult( null );
                        using( await ets.GetExecutionToken( ) ) {
                           // nop
                        }
                     } );

         Task.WaitAll( syncTasks );

         ets.Dispose( );
         resetEvent.Set( );

         AggregateException exc = null;
         try {
            Task.WaitAll( tasks.ToArray( ) );
         } catch( AggregateException exc2 ) {
            exc = exc2;
         }

         ClassicAssert.NotNull( exc );
         ClassicAssert.AreEqual( 1, exc.InnerExceptions.Count );
         ClassicAssert.AreEqual( typeof( ExecutionTokenSpoolDisposedException ), exc.InnerExceptions.First( ).GetType( ) );
      }

      [Test]
      public void ExecutionsDoNotIntersect( ) {
         ExecutionTokenSpool ets = new ExecutionTokenSpool( );
         List<Task> tasks = new List<Task>( 5 );
         List<FromToSpan> spans = new List<FromToSpan>( 5 );
         for( int i = 0; i < 5; i++ )
            tasks.Add(
                  Task.Run(
                        async ( ) => {
                           FromToSpan span = new FromToSpan( );
                           using( await ets.GetExecutionToken( ) ) {
                              span.From = DateTime.Now;
                              await Task.Delay( 10 );
                              span.To = DateTime.Now;
                           }

                           lock( spans ) {
                              spans.Add( span );
                           }
                        } ) );

         Task.WaitAll( tasks.ToArray( ) );

         ClassicAssert.AreEqual( 5, spans.Count );
         ClassicAssert.False( spans.Any( s1 => spans.Any( s2 => s1 != s2&&s1.Intersects( s2 ) ) ) );
      }

      [Test]
      public async Task MostBasicTest( ) {
         ExecutionTokenSpool ets = new ExecutionTokenSpool( );
         using( await ets.GetExecutionToken( ) ) {
            // nop
         }
      }

   }

}
