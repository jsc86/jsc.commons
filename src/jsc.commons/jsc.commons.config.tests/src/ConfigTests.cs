﻿// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using jsc.commons.config.interfaces;

using NUnit.Framework;
using NUnit.Framework.Legacy;

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
         ClassicAssert.AreEqual( "read string", conf.StringProp1 );
         ClassicAssert.AreEqual( 12345, conf.IntProp );
         ClassicAssert.AreEqual( "Hello World!", conf.StringProp2 );
      }

      [Test]
      public void ConfigBackendSave( ) {
         TestBackend backend = new TestBackend( );
         ITestConfig conf = Config.New<ITestConfig>( );
         conf.Backend = backend;
         conf.StringProp1 = "saved string";
         conf.IntProp = 12345;
         conf.Save( );
         ClassicAssert.AreEqual( "saved string", backend.Values[ "StringProp1" ] );
         ClassicAssert.AreEqual( 12345, backend.Values[ "IntProp" ] );
         ClassicAssert.AreEqual( conf.Keys.Count( ), backend.Values.Count );
      }

      [Test]
      public void HasDefaultFromDefaultProvider( ) {
         ITestConfig conf = Config.New<ITestConfig>( );
         ClassicAssert.AreEqual( CultureInfo.InvariantCulture, conf.Culture );
      }

      [Test]
      public void HasDefaultIntValue( ) {
         ITestConfig conf = Config.New<ITestConfig>( );
         ClassicAssert.AreEqual( 42, conf.IntProp );
      }

      [Test]
      public void HasDefaultValue( ) {
         ITestConfig conf = Config.New<ITestConfig>( );
         ClassicAssert.AreEqual( "Hello World!", conf.StringProp2 );
      }

      [Test]
      public void HasNoDefaultValue( ) {
         ITestConfig conf = Config.New<ITestConfig>( );
         ClassicAssert.IsNull( conf.StringProp1 );
      }

      [Test]
      public void SetAndReadValue( ) {
         ITestConfig conf = Config.New<ITestConfig>( );
         conf.StringProp1 = "test value";
         ClassicAssert.AreEqual( "test value", conf.StringProp1 );
      }

      [Test]
      public void ValidInterface( ) {
         ITestConfig conf = Config.New<ITestConfig>( );
         ClassicAssert.IsNotNull( conf );
      }

      public class DefaultsPerInstanceDefaultsProvider : DefaultsProviderBase {

         public DefaultsPerInstanceDefaultsProvider( ) : base(
               new[] {
                     new Tuple<string, Func<object>>(
                           nameof( IDefaultsPerInstanceConf.MyList ),
                           ( ) => new List<string>( ) )
               } ) { }

      }

      [Config( DefaultsProvider = typeof( DefaultsPerInstanceDefaultsProvider ) )]
      public interface IDefaultsPerInstanceConf : IConfiguration {

         [ConfigValue]
         IList<string> MyList { get; set; }

      }

      [Test]
      public void DefaultsPerInstance( ) {
         IDefaultsPerInstanceConf conf1 = Config.New<IDefaultsPerInstanceConf>( );
         IDefaultsPerInstanceConf conf2 = Config.New<IDefaultsPerInstanceConf>( );
         conf1.MyList.Add( "asdf" );

         ClassicAssert.AreEqual( 1, conf1.MyList.Count );
         ClassicAssert.AreEqual( 0, conf2.MyList.Count );

         IDefaultsPerInstanceConf conf3 = Config.New<IDefaultsPerInstanceConf>( );

         ClassicAssert.AreEqual( 0, conf3.MyList.Count );
      }

   }

}
