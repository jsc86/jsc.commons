// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using jsc.commons.cli.ispec;
using jsc.commons.cli.ispec.attrib;
using jsc.commons.config.interfaces;

using NUnit.Framework;

namespace jsc.commons.cli.tests.ispec {

   [TestFixture]
   public class SpecDeriverBasicTests {

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

      [CliDefinition]
      public interface ICliDynArg : IConfiguration {

         [Argument( Dynamic = true )]
         IEnumerable<int> Arg { get; set; }

      }

      [CliDefinition]
      public interface ICliDynArgOption : IConfiguration {

         [Option]
         [FirstArgument( Name = "int", Dynamic = true )]
         IEnumerable<int> Arg { get; set; }

      }

      [CliDefinition]
      public interface ICliNullableInt : IConfiguration {

         [Argument]
         int? Arg { get; set; }

      }

      [Test]
      public void DynArg( ) {
         ICliDynArg cli = new InterfaceSpecBoilerPlateHelper<ICliDynArg>(
               new[] {
                     1.ToString( ),
                     2.ToString( ),
                     3.ToString( )
               } ).CliConfigObject;

         Assert.IsNotNull( cli.Arg );

         List<int> argValues = cli.Arg.ToList( );

         Assert.AreEqual( 3, argValues.Count );
         Assert.AreEqual( 1, argValues[ 0 ] );
         Assert.AreEqual( 2, argValues[ 1 ] );
         Assert.AreEqual( 3, argValues[ 2 ] );
      }

      [Test]
      public void DynArg_Empty( ) {
         ICliDynArg cli = new InterfaceSpecBoilerPlateHelper<ICliDynArg>(
               new string[0] ).CliConfigObject;

         Assert.IsNotNull( cli.Arg );

         List<int> argValues = cli.Arg.ToList( );

         Assert.AreEqual( 0, argValues.Count );
      }

      [Test]
      public void DynArgOption( ) {
         ICliDynArgOption cli = new InterfaceSpecBoilerPlateHelper<ICliDynArgOption>(
               new[] {
                     "--arg",
                     1.ToString( ),
                     2.ToString( ),
                     3.ToString( )
               } ).CliConfigObject;

         Assert.IsNotNull( cli.Arg );

         List<int> argValues = cli.Arg.ToList( );

         Assert.AreEqual( 3, argValues.Count );
         Assert.AreEqual( 1, argValues[ 0 ] );
         Assert.AreEqual( 2, argValues[ 1 ] );
         Assert.AreEqual( 3, argValues[ 2 ] );
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
                     "1",
                     "2",
                     "3",
                     "-t",
                     "another string",
                     "42",
                     "-m"
               } ).CliConfigObject;

         Assert.AreEqual( "a string", cli.StringOptionOne );
         Assert.AreEqual( "another string", cli.StringOptionTwo );
         Assert.AreEqual( 42, cli.StringOptionTwoCount );
         Assert.AreEqual( true, cli.MyFlag );
         Assert.AreEqual( 3, cli.StringOptionOneDynIntArg.Count( ) );
         Assert.AreEqual( 1, cli.StringOptionOneDynIntArg.First( ) );
         Assert.AreEqual( 3, cli.StringOptionOneDynIntArg.Last( ) );
      }

      [Test]
      public void NullableInt_NotSet( ) {
         ICliNullableInt cli = new InterfaceSpecBoilerPlateHelper<ICliNullableInt>(
               new string[0] ).CliConfigObject;

         Assert.IsFalse( cli.Arg.HasValue );
      }

      [Test]
      public void NullableInt_Set( ) {
         ICliNullableInt cli = new InterfaceSpecBoilerPlateHelper<ICliNullableInt>(
               new[] {
                     42.ToString( )
               } ).CliConfigObject;

         Assert.IsTrue( cli.Arg.HasValue );
         Assert.AreEqual( 42, cli.Arg );
      }

   }

}