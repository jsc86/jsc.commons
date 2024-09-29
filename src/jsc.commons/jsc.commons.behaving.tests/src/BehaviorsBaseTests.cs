// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.behaving.interfaces;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace jsc.commons.behaving.tests {

   [TestFixture]
   public class BehaviorsBaseTests {

      [Test]
      public void Get_BehaviorContained( ) {
         IBehaviors behaviors = new BehaviorsBase( );
         behaviors.Set( new TestBehavior( ) );
         Exception exc = null;
         TestBehavior behavior = null;
         try {
            behavior = behaviors.Get<TestBehavior>( );
         } catch( Exception exc2 ) {
            exc = exc2;
         }

         ClassicAssert.IsNotNull( behavior );
         ClassicAssert.IsNull( exc );
      }

      [Test]
      public void Get_BehaviorNotContained( ) {
         IBehaviors behaviors = new BehaviorsBase( );
         Exception exc = null;
         TestBehavior behavior = null;
         try {
            behavior = behaviors.Get<TestBehavior>( );
         } catch( Exception exc2 ) {
            exc = exc2;
         }

         ClassicAssert.IsNull( behavior );
         ClassicAssert.IsNotNull( exc );
         ClassicAssert.IsInstanceOf<ArgumentException>( exc );
      }

      [Test]
      public void Get_BehaviorReplaced( ) {
         IBehaviors behaviors = new BehaviorsBase( );
         TestBehavior behavior1 = new TestBehavior( );
         TestBehavior behavior2 = new TestBehavior( );
         behaviors.Set( behavior1 );
         behaviors.Set( behavior2 );
         TestBehavior behavior = behaviors.Get<TestBehavior>( );

         ClassicAssert.IsNotNull( behavior );
         ClassicAssert.AreSame( behavior2, behavior );
      }

      [Test]
      public void TryGet_BehaviorContained( ) {
         IBehaviors behaviors = new BehaviorsBase( );
         behaviors.Set( new TestBehavior( ) );
         bool found = behaviors.TryGet( out TestBehavior behavior );

         ClassicAssert.IsTrue( found );
         ClassicAssert.IsNotNull( behavior );
      }

      [Test]
      public void TryGet_BehaviorNotContained( ) {
         IBehaviors behaviors = new BehaviorsBase( );
         bool found = behaviors.TryGet( out TestBehavior behavior );

         ClassicAssert.IsFalse( found );
         ClassicAssert.IsNull( behavior );
      }

   }

}
