// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.behaving.interfaces;

using NUnit.Framework;

namespace jsc.commons.behaving.tests {

   [TestFixture]
   public class BehaviorsContainerBaseTests {

      [Test]
      public void Get_BehaviorContained( ) {
         IBehaviorsContainer behaviors = new BehaviorsContainerBase( );
         behaviors.Set( new TestBehavior( ) );
         Exception exc = null;
         TestBehavior behavior = null;
         try {
            behavior = behaviors.Get<TestBehavior>( );
         } catch( Exception exc2 ) {
            exc = exc2;
         }

         Assert.IsNotNull( behavior );
         Assert.IsNull( exc );
      }

      [Test]
      public void Get_BehaviorNotContained( ) {
         IBehaviorsContainer behaviors = new BehaviorsContainerBase( );
         Exception exc = null;
         TestBehavior behavior = null;
         try {
            behavior = behaviors.Get<TestBehavior>( );
         } catch( Exception exc2 ) {
            exc = exc2;
         }

         Assert.IsNull( behavior );
         Assert.IsNotNull( exc );
         Assert.IsInstanceOf<ArgumentException>( exc );
      }

      [Test]
      public void Get_BehaviorReplaced( ) {
         IBehaviorsContainer behaviors = new BehaviorsContainerBase( );
         TestBehavior behavior1 = new TestBehavior( );
         TestBehavior behavior2 = new TestBehavior( );
         behaviors.Set( behavior1 );
         behaviors.Set( behavior2 );
         TestBehavior behavior = behaviors.Get<TestBehavior>( );

         Assert.IsNotNull( behavior );
         Assert.AreSame( behavior2, behavior );
      }

      [Test]
      public void TryGet_BehaviorContained( ) {
         IBehaviorsContainer behaviors = new BehaviorsContainerBase( );
         behaviors.Set( new TestBehavior( ) );
         bool found = behaviors.TryGet( out TestBehavior behavior );

         Assert.IsTrue( found );
         Assert.IsNotNull( behavior );
      }

      [Test]
      public void TryGet_BehaviorNotContained( ) {
         IBehaviorsContainer behaviors = new BehaviorsContainerBase( );
         bool found = behaviors.TryGet( out TestBehavior behavior );

         Assert.IsFalse( found );
         Assert.IsNull( behavior );
      }

   }

}