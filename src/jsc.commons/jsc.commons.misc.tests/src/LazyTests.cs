// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace jsc.commons.misc.tests {

   [TestFixture]
   public class LazyTests {

      [SetUp]
      public void Init( ) {
         TestClass.Instances.Clear( );
      }

      [Test]
      public void ExplicitSpawner( ) {
         ClassicAssert.AreEqual( 0, TestClass.Instances.Count );
         Lazy<TestClass> l = new Lazy<TestClass>( ( ) => new TestClass( ) );
         ClassicAssert.AreEqual( 0, TestClass.Instances.Count );
         ClassicAssert.False( l.IsInitialized );
         ClassicAssert.IsNotNull( l.Instance );
         ClassicAssert.AreEqual( 1, TestClass.Instances.Count );
      }

      [Test]
      public void ImplicitSpawner( ) {
         ClassicAssert.AreEqual( 0, TestClass.Instances.Count );
         Lazy<TestClass> l = new Lazy<TestClass>( );
         ClassicAssert.AreEqual( 0, TestClass.Instances.Count );
         ClassicAssert.False( l.IsInitialized );
         ClassicAssert.IsNotNull( l.Instance );
         ClassicAssert.AreEqual( 1, TestClass.Instances.Count );
      }

   }

   internal class TestClass {

      public static readonly List<TestClass> Instances = new List<TestClass>( );

      public TestClass( ) {
         Instances.Add( this );
      }

   }

}
