// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;

using jsc.commons.cli.arguments;
using jsc.commons.cli.config;
using jsc.commons.cli.interfaces;
using jsc.commons.cli.parser;
using jsc.commons.config;
using jsc.commons.naming;

using NUnit.Framework;

namespace jsc.commons.cli.tests {

   [TestFixture]
   public class CommandLineParserTests {

      public void TestTemplate1(
            string[] args,
            bool arg1ShouldBeSet,
            bool arg2ShouldBeSet,
            bool optShouldBeSet,
            bool flagShouldBeSet,
            string arg1ShouldBe = null,
            string arg2ShouldBe = null ) {
         IArgument arg1 = new Argument<string>( "Argument 1", null );
         IArgument arg2 = new Argument<string>( "Argument 2", null );
         CliSpecification spec = new CliSpecification(
               args: new[] {
                     arg1
               } );
         IFlag flag = new Flag( 'x' );
         spec.Flags.Add( flag );
         IOption opt = new Option(
               new UnifiedName( "my", "opt" ),
               true,
               'm',
               null,
               arg2 );
         spec.Options.Add( opt );

         ICommandLineParser clp = new CommandLineParser( spec );

         IParserResult pr = clp.Parse( args );

         Assert.AreEqual( arg1ShouldBeSet, pr.IsSet( arg1 ) );
         Assert.AreEqual( arg2ShouldBeSet, pr.IsSet( arg2 ) );
         Assert.AreEqual( optShouldBeSet, pr.IsSet( opt ) );
         Assert.AreEqual( flagShouldBeSet, pr.IsSet( flag ) );
         if( arg1ShouldBe != null )
            Assert.AreEqual( arg1ShouldBe, pr.GetValue( arg1 ) );
         if( arg2ShouldBe != null )
            Assert.AreEqual( arg2ShouldBe, pr.GetValue( arg2 ) );
      }

      [Test]
      public void AutoAddedHelpOption_IsNotSet( ) {
         ICliSpecification spec = new CliSpecification( );
         CommandLineParser clp = new CommandLineParser( spec );
         IParserResult pr = clp.Parse( new string[0] );

         Assert.IsNotNull( spec.HelpOption );
         Assert.IsFalse( pr.IsSet( spec.HelpOption ) );
      }

      [Test]
      public void AutoAddedHelpOption_IsSet( ) {
         ICliSpecification spec = new CliSpecification( );
         CommandLineParser clp = new CommandLineParser( spec );
         IParserResult pr = clp.Parse( new[] {"--help"} );

         Assert.IsNotNull( spec.HelpOption );
         Assert.IsTrue( pr.IsSet( spec.HelpOption ) );
      }

      [Test]
      public void AutoAddedHelpOption_IsSetFlag( ) {
         ICliSpecification spec = new CliSpecification( );
         CommandLineParser clp = new CommandLineParser( spec );
         IParserResult pr = clp.Parse( new[] {"-h"} );

         Assert.IsNotNull( spec.HelpOption );
         Assert.IsTrue( pr.IsSet( spec.HelpOption ) );
      }

      [Test]
      public void DynamicArgumentTest( ) {
         IArgument<string> dynStringArg = new StringArg( "dyn arg", null );
         CliSpecification spec =
               new CliSpecification(
                     null,
                     dynStringArg );

         ICommandLineParser clp = new CommandLineParser( spec );

         IParserResult pr =
               clp.Parse(
                     new[] {
                           "arg1",
                           "arg2",
                           "arg3"
                     } );

         List<string> dynArgValues = pr.GetDynamicValues( dynStringArg ).ToList( );

         Assert.AreEqual( 3, dynArgValues.Count );
         Assert.AreEqual( "arg1", dynArgValues[ 0 ] );
         Assert.AreEqual( "arg2", dynArgValues[ 1 ] );
         Assert.AreEqual( "arg3", dynArgValues[ 2 ] );
      }

      [Test]
      public void DynamicArgumentTest2( ) {
         IArgument<string> dynStringArg = new StringArg( "dyn arg", null );
         CliSpecification spec =
               new CliSpecification(
                     null,
                     dynStringArg );

         IArgument<int> dynIntArg = new Argument<int>( "dyn int arg", null );

         IOption myOpt = new Option(
               new UnifiedName( "my", "opt" ),
               false,
               null,
               dynIntArg );

         spec.Options.Add( myOpt );

         ICommandLineParser clp = new CommandLineParser( spec );

         IParserResult pr =
               clp.Parse(
                     new[] {
                           "arg1",
                           "arg2",
                           "--my-opt",
                           "23",
                           "42"
                     } );

         List<string> dynArgValues = pr.GetDynamicValues( dynStringArg ).ToList( );

         Assert.AreEqual( 2, dynArgValues.Count );
         Assert.AreEqual( "arg1", dynArgValues[ 0 ] );
         Assert.AreEqual( "arg2", dynArgValues[ 1 ] );

         List<int> dynIntArgValues = pr.GetDynamicValues( dynIntArg ).ToList( );

         Assert.AreEqual( 2, dynIntArgValues.Count );
         Assert.AreEqual( 23, dynIntArgValues[ 0 ] );
         Assert.AreEqual( 42, dynIntArgValues[ 1 ] );
      }


      [Test]
      public void FlagCaseSensitivityCorrectCase( ) {
         ICliSpecification spec = new CliSpecification( );
         IFlag flag = new Flag( 'f' );
         spec.Flags.Add( flag );
         CommandLineParser clp = new CommandLineParser( spec );
         IParserResult pr = clp.Parse( new[] {"-f"} );
         Assert.IsTrue( pr.IsSet( flag ) );
      }

      [Test]
      public void FlagCaseSensitivityIncorrectCase( ) {
         ICliSpecification spec = new CliSpecification( );
         IFlag flag = new Flag( 'f' );
         spec.Flags.Add( flag );
         CommandLineParser clp = new CommandLineParser( spec );
         Exception exc = null;
         try {
            clp.Parse( new[] {"-F"} );
         } catch( Exception ex ) {
            exc = ex;
         }

         Assert.IsNotNull( exc );
         Assert.IsTrue( exc.Message.StartsWith( "unknown flag" ) );
      }

      [Test]
      public void OptionCaseSensitivityCorrectCase( ) {
         ICliSpecification spec = new CliSpecification( );
         IOption opt = new Option( new UnifiedName( "opt" ) );
         spec.Options.Add( opt );
         CommandLineParser clp = new CommandLineParser( spec );
         IParserResult pr = clp.Parse( new[] {"--opt"} );
         Assert.IsTrue( pr.IsSet( opt ) );
      }

      [Test]
      public void OptionCaseSensitivityIgnoreCaseLower( ) {
         ICliConfig conf = Config.New<ICliConfig>( );
         conf.CaseSensitiveOptions = false;
         ICliSpecification spec = new CliSpecification( conf );
         IOption opt = new Option( new UnifiedName( "opt" ) );
         spec.Options.Add( opt );
         CommandLineParser clp = new CommandLineParser( spec );
         IParserResult pr = clp.Parse( new[] {"--opt"} );
         Assert.IsTrue( pr.IsSet( opt ) );
      }

      [Test]
      public void OptionCaseSensitivityIgnoreCaseLowerLower( ) {
         ICliConfig conf = Config.New<ICliConfig>( );
         conf.CaseSensitiveFlags = false;
         ICliSpecification spec = new CliSpecification( conf );
         IFlag flag = new Flag( 'f' );
         spec.Flags.Add( flag );
         CommandLineParser clp = new CommandLineParser( spec );
         IParserResult pr = clp.Parse( new[] {"-f"} );
         Assert.IsTrue( pr.IsSet( flag ) );
      }

      [Test]
      public void OptionCaseSensitivityIgnoreCaseLowerUpper( ) {
         ICliConfig conf = Config.New<ICliConfig>( );
         conf.CaseSensitiveFlags = false;
         ICliSpecification spec = new CliSpecification( conf );
         IFlag flag = new Flag( 'f' );
         spec.Flags.Add( flag );
         CommandLineParser clp = new CommandLineParser( spec );
         IParserResult pr = clp.Parse( new[] {"-F"} );
         Assert.IsTrue( pr.IsSet( flag ) );
      }

      [Test]
      public void OptionCaseSensitivityIgnoreCaseMixed( ) {
         ICliConfig conf = Config.New<ICliConfig>( );
         conf.CaseSensitiveOptions = false;
         ICliSpecification spec = new CliSpecification( conf );
         IOption opt = new Option( new UnifiedName( "opt" ) );
         spec.Options.Add( opt );
         CommandLineParser clp = new CommandLineParser( spec );
         IParserResult pr = clp.Parse( new[] {"--Opt"} );
         Assert.IsTrue( pr.IsSet( opt ) );
      }

      [Test]
      public void OptionCaseSensitivityIgnoreCaseUpper( ) {
         ICliConfig conf = Config.New<ICliConfig>( );
         conf.CaseSensitiveOptions = false;
         ICliSpecification spec = new CliSpecification( conf );
         IOption opt = new Option( new UnifiedName( "opt" ) );
         spec.Options.Add( opt );
         CommandLineParser clp = new CommandLineParser( spec );
         IParserResult pr = clp.Parse( new[] {"--OPT"} );
         Assert.IsTrue( pr.IsSet( opt ) );
      }

      [Test]
      public void OptionCaseSensitivityIgnoreCaseUpperLower( ) {
         ICliConfig conf = Config.New<ICliConfig>( );
         conf.CaseSensitiveFlags = false;
         ICliSpecification spec = new CliSpecification( conf );
         IFlag flag = new Flag( 'F' );
         spec.Flags.Add( flag );
         CommandLineParser clp = new CommandLineParser( spec );
         IParserResult pr = clp.Parse( new[] {"-f"} );
         Assert.IsTrue( pr.IsSet( flag ) );
      }

      [Test]
      public void OptionCaseSensitivityIgnoreCaseUpperUpper( ) {
         ICliConfig conf = Config.New<ICliConfig>( );
         conf.CaseSensitiveFlags = false;
         ICliSpecification spec = new CliSpecification( conf );
         IFlag flag = new Flag( 'F' );
         spec.Flags.Add( flag );
         CommandLineParser clp = new CommandLineParser( spec );
         IParserResult pr = clp.Parse( new[] {"-F"} );
         Assert.IsTrue( pr.IsSet( flag ) );
      }

      [Test]
      public void OptionCaseSensitivityIncorrectCase( ) {
         ICliSpecification spec = new CliSpecification( );
         IOption opt = new Option( new UnifiedName( "opt" ) );
         spec.Options.Add( opt );
         CommandLineParser clp = new CommandLineParser( spec );
         Exception exc = null;
         try {
            clp.Parse( new[] {"--OPT"} );
         } catch( Exception ex ) {
            exc = ex;
         }

         Assert.IsNotNull( exc );
         Assert.IsTrue( exc.Message.StartsWith( "unknown option" ) );
      }

      [Test]
      public void Test1( ) {
         TestTemplate1( new string[0], false, false, false, false );
      }

      [Test]
      public void Test2( ) {
         TestTemplate1(
               new[] {
                     "asdf"
               },
               true,
               false,
               false,
               false,
               "asdf" );
      }

      [Test]
      public void Test3( ) {
         TestTemplate1(
               new[] {
                     "-x"
               },
               false,
               false,
               false,
               true );
      }

      [Test]
      public void Test4( ) {
         TestTemplate1(
               new[] {
                     "--my-opt"
               },
               false,
               false,
               true,
               false );
      }

      [Test]
      public void Test5( ) {
         TestTemplate1(
               new[] {
                     "-m"
               },
               false,
               false,
               true,
               false );
      }

      [Test]
      public void Test6( ) {
         TestTemplate1(
               new[] {
                     "asdf",
                     "-m"
               },
               true,
               false,
               true,
               false,
               "asdf" );
      }

      [Test]
      public void Test7( ) {
         TestTemplate1(
               new[] {
                     "-m",
                     "asdf"
               },
               false,
               true,
               true,
               false,
               arg2ShouldBe: "asdf" );
      }

      [Test]
      public void Test8( ) {
         TestTemplate1(
               new[] {
                     "asdf",
                     "-m",
                     "qwert"
               },
               true,
               true,
               true,
               false,
               "asdf",
               "qwert" );
      }

      [Test]
      public void Test9( ) {
         TestTemplate1(
               new[] {
                     "-m",
                     "qwert",
                     "asdf"
               },
               true,
               true,
               true,
               false,
               "asdf",
               "qwert" );
      }

   }

}