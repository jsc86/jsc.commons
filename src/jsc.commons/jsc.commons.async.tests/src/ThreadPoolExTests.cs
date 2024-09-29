// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Threading.Tasks;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace jsc.commons.async.tests {

   [TestFixture]
   public class ThreadPoolExTests {

      [Test]
      public async Task Test1( ) {
         bool wasInvoked = false;
         await ThreadPoolEx.InvokeAsync(
               ( ) => { wasInvoked = true; } );

         ClassicAssert.IsTrue( wasInvoked );
      }

      [Test]
      public async Task Test2( ) {
         bool wasInvoked = false;
         int n = await ThreadPoolEx.InvokeAsync(
               ( ) => {
                  wasInvoked = true;
                  return 42;
               } );

         ClassicAssert.IsTrue( wasInvoked );
         ClassicAssert.AreEqual( 42, n );
      }

      [Test]
      public async Task Test3( ) {
         bool wasInvoked = false;
         await ThreadPoolEx.InvokeAsync(
               async ( ) => {
                  await ThreadPoolEx.InvokeAsync(
                        ( ) => { wasInvoked = true; } );
               } );
         ClassicAssert.IsTrue( wasInvoked );
      }

      [Test]
      public async Task Test4( ) {
         bool wasInvoked = false;
         int n = await ThreadPoolEx.InvokeAsync(
               async ( ) => await ThreadPoolEx.InvokeAsync(
                     ( ) => {
                        wasInvoked = true;
                        return 42;
                     } ) );
         ClassicAssert.IsTrue( wasInvoked );
         ClassicAssert.AreEqual( 42, n );
      }

   }

}
