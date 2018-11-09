// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.cli.interfaces;
using jsc.commons.cli.parser;
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
         IArgument arg1 = new Argument<string>( "Argument 1" );
         IArgument arg2 = new Argument<string>( "Argument 2" );
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
