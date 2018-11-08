// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Globalization;

using jsc.commons.cli.ispec;
using jsc.commons.cli.ispec.attrib;
using jsc.commons.config.interfaces;

using NUnit.Framework;

namespace jsc.commons.cli.tests {

   [TestFixture]
   public class SpecDeriverTests {

      [CliDefinition]
      public interface ICliIntArg : IConfiguration {

         [Argument]
         int Arg { get; set; }

      }

      [CliDefinition]
      public interface ICliLongArg : IConfiguration {

         [Argument]
         long Arg { get; set; }

      }

      [CliDefinition]
      public interface ICliFloatArg : IConfiguration {

         [Argument]
         float Arg { get; set; }

      }

      [Test]
      public void FloatArg_NegativeValue( ) {
         const float f = -123.456789f;
         ICliFloatArg cli = new InterfaceSpecBoilerPlateHelper<ICliFloatArg>(
               new[] {
                     f.ToString( CultureInfo.InvariantCulture )
               } ).CliConfigObject;

         Assert.True( Math.Abs( f-cli.Arg ) < 0.0001 );
      }

      [Test]
      public void FloatArg_PositiveValue( ) {
         const float f = 123.456789f;
         ICliFloatArg cli = new InterfaceSpecBoilerPlateHelper<ICliFloatArg>(
               new[] {
                     f.ToString( CultureInfo.InvariantCulture )
               } ).CliConfigObject;

         Assert.True( Math.Abs( f-cli.Arg ) < 0.0001 );
      }

      [Test]
      public void FloatArg_Zero( ) {
         ICliFloatArg cli = new InterfaceSpecBoilerPlateHelper<ICliFloatArg>(
               new[] {
                     0f.ToString( CultureInfo.InvariantCulture )
               } ).CliConfigObject;

         Assert.AreEqual( 0f, cli.Arg );
      }

      [Test]
      public void IntArg_MaxValue( ) {
         ICliIntArg cli = new InterfaceSpecBoilerPlateHelper<ICliIntArg>(
               new[] {
                     int.MaxValue.ToString( )
               } ).CliConfigObject;

         Assert.AreEqual( int.MaxValue, cli.Arg );
      }

      [Test]
      public void IntArg_MinValue( ) {
         ICliIntArg cli = new InterfaceSpecBoilerPlateHelper<ICliIntArg>(
               new[] {
                     int.MinValue.ToString( )
               } ).CliConfigObject;

         Assert.AreEqual( int.MinValue, cli.Arg );
      }

      [Test]
      public void IntArg_Zero( ) {
         ICliIntArg cli = new InterfaceSpecBoilerPlateHelper<ICliIntArg>(
               new[] {
                     0.ToString( )
               } ).CliConfigObject;

         Assert.AreEqual( 0, cli.Arg );
      }

      [Test]
      public void LongArg_MaxValue( ) {
         ICliLongArg cli = new InterfaceSpecBoilerPlateHelper<ICliLongArg>(
               new[] {
                     long.MaxValue.ToString( )
               } ).CliConfigObject;

         Assert.AreEqual( long.MaxValue, cli.Arg );
      }

      [Test]
      public void LongArg_MinValue( ) {
         ICliLongArg cli = new InterfaceSpecBoilerPlateHelper<ICliLongArg>(
               new[] {
                     long.MinValue.ToString( )
               } ).CliConfigObject;

         Assert.AreEqual( long.MinValue, cli.Arg );
      }

      [Test]
      public void LongArg_Zero( ) {
         ICliLongArg cli = new InterfaceSpecBoilerPlateHelper<ICliLongArg>(
               new[] {
                     0.ToString( )
               } ).CliConfigObject;

         Assert.AreEqual( 0, cli.Arg );
      }

      [Test]
      public void MonsterTest( ) {
         ICliTest cli = new InterfaceSpecBoilerPlateHelper<ICliTest>(
               new[] {
                     "--string-option-one",
                     "a string",
                     "-t",
                     "another string",
                     "42",
                     "-m"
               } ).CliConfigObject;

         Assert.AreEqual( "a string", cli.StringOptionOne );
         Assert.AreEqual( "another string", cli.StringOptionTwo );
         Assert.AreEqual( 42, cli.StringOptionTwoCount );
         Assert.AreEqual( true, cli.MyFlag );
      }

   }

}