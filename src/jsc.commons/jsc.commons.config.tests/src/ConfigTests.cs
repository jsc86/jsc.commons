// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Globalization;
using System.Linq;

using NUnit.Framework;

namespace jsc.commons.config.tests {

   [TestFixture]
   public class ConfigTests {

      [Test]
      public void ConfigBackendRead( ) {
         TestBackend backend = new TestBackend( );
         backend.Values[ "StringProp1" ] = "read string";
         backend.Values[ "IntProp" ] = 12345;
         ITestConfig conf = Config.New<ITestConfig>( );
         conf.Backend = backend;
         conf.Read( );
         Assert.AreEqual( "read string", conf.StringProp1 );
         Assert.AreEqual( 12345, conf.IntProp );
         Assert.AreEqual( "Hello World!", conf.StringProp2 );
      }

      [Test]
      public void ConfigBackendSave( ) {
         TestBackend backend = new TestBackend( );
         ITestConfig conf = Config.New<ITestConfig>( );
         conf.Backend = backend;
         conf.StringProp1 = "saved string";
         conf.IntProp = 12345;
         conf.Save( );
         Assert.AreEqual( "saved string", backend.Values[ "StringProp1" ] );
         Assert.AreEqual( 12345, backend.Values[ "IntProp" ] );
         Assert.AreEqual( conf.Keys.Count( ), backend.Values.Count );
      }

      [Test]
      public void HasDefaultFromDefaultProvider( ) {
         ITestConfig conf = Config.New<ITestConfig>( );
         Assert.AreEqual( CultureInfo.InvariantCulture, conf.Culture );
      }

      [Test]
      public void HasDefaultIntValue( ) {
         ITestConfig conf = Config.New<ITestConfig>( );
         Assert.AreEqual( 42, conf.IntProp );
      }

      [Test]
      public void HasDefaultValue( ) {
         ITestConfig conf = Config.New<ITestConfig>( );
         Assert.AreEqual( "Hello World!", conf.StringProp2 );
      }

      [Test]
      public void HasNoDefaultValue( ) {
         ITestConfig conf = Config.New<ITestConfig>( );
         Assert.IsNull( conf.StringProp1 );
      }

      [Test]
      public void SetAndReadValue( ) {
         ITestConfig conf = Config.New<ITestConfig>( );
         conf.StringProp1 = "test value";
         Assert.AreEqual( "test value", conf.StringProp1 );
      }

      [Test]
      public void ValidInterface( ) {
         ITestConfig conf = Config.New<ITestConfig>( );
         Assert.IsNotNull( conf );
      }

   }

}