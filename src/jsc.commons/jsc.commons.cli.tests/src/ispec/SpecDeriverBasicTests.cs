// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
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
using NUnit.Framework.Legacy;

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

      public enum MyEnum {

         Value1,
         Value2

      }

      [CliDefinition]
      public interface ICliEnumArg : IConfiguration {

         [Argument]
         MyEnum Arg { get; set; }

      }

      [CliDefinition]
      public interface ICliNullableEnumArg : IConfiguration {

         [Argument]
         MyEnum? Arg { get; set; }

      }

      [Test]
      public void DynArg( ) {
         ICliDynArg cli = new InterfaceSpecBoilerPlateHelper<ICliDynArg>(
               new[] {
                     1.ToString( ),
                     2.ToString( ),
                     3.ToString( )
               } ).CliConfigObject;

         ClassicAssert.IsNotNull( cli.Arg );

         List<int> argValues = cli.Arg.ToList( );

         ClassicAssert.AreEqual( 3, argValues.Count );
         ClassicAssert.AreEqual( 1, argValues[ 0 ] );
         ClassicAssert.AreEqual( 2, argValues[ 1 ] );
         ClassicAssert.AreEqual( 3, argValues[ 2 ] );
      }

      [Test]
      public void DynArg_Empty( ) {
         ICliDynArg cli = new InterfaceSpecBoilerPlateHelper<ICliDynArg>(
               new string[0] ).CliConfigObject;

         ClassicAssert.IsNotNull( cli.Arg );

         List<int> argValues = cli.Arg.ToList( );

         ClassicAssert.AreEqual( 0, argValues.Count );
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

         ClassicAssert.IsNotNull( cli.Arg );

         List<int> argValues = cli.Arg.ToList( );

         ClassicAssert.AreEqual( 3, argValues.Count );
         ClassicAssert.AreEqual( 1, argValues[ 0 ] );
         ClassicAssert.AreEqual( 2, argValues[ 1 ] );
         ClassicAssert.AreEqual( 3, argValues[ 2 ] );
      }

      [Test]
      public void EnumArg_IsSet( ) {
         ICliEnumArg cli = new InterfaceSpecBoilerPlateHelper<ICliEnumArg>(
               new[] {
                     MyEnum.Value1.ToString( )
               } ).CliConfigObject;

         ClassicAssert.AreEqual( MyEnum.Value1, cli.Arg );
      }

      [Test]
      public void EnumArg_IsSetLowerCase( ) {
         ICliEnumArg cli = new InterfaceSpecBoilerPlateHelper<ICliEnumArg>(
               new[] {
                     MyEnum.Value2.ToString( ).ToLower( )
               } ).CliConfigObject;

         ClassicAssert.AreEqual( MyEnum.Value2, cli.Arg );
      }

      [Test]
      public void FloatArg_NegativeValue( ) {
         const float f = -123.456789f;
         ICliFloatArg cli = new InterfaceSpecBoilerPlateHelper<ICliFloatArg>(
               new[] {
                     f.ToString( CultureInfo.InvariantCulture )
               } ).CliConfigObject;

         ClassicAssert.True( Math.Abs( f-cli.Arg ) < 0.0001 );
      }

      [Test]
      public void FloatArg_PositiveValue( ) {
         const float f = 123.456789f;
         ICliFloatArg cli = new InterfaceSpecBoilerPlateHelper<ICliFloatArg>(
               new[] {
                     f.ToString( CultureInfo.InvariantCulture )
               } ).CliConfigObject;

         ClassicAssert.True( Math.Abs( f-cli.Arg ) < 0.0001 );
      }

      [Test]
      public void FloatArg_Zero( ) {
         ICliFloatArg cli = new InterfaceSpecBoilerPlateHelper<ICliFloatArg>(
               new[] {
                     0f.ToString( CultureInfo.InvariantCulture )
               } ).CliConfigObject;

         ClassicAssert.AreEqual( 0f, cli.Arg );
      }

      [Test]
      public void IntArg_MaxValue( ) {
         ICliIntArg cli = new InterfaceSpecBoilerPlateHelper<ICliIntArg>(
               new[] {
                     int.MaxValue.ToString( )
               } ).CliConfigObject;

         ClassicAssert.AreEqual( int.MaxValue, cli.Arg );
      }

      [Test]
      public void IntArg_MinValue( ) {
         ICliIntArg cli = new InterfaceSpecBoilerPlateHelper<ICliIntArg>(
               new[] {
                     int.MinValue.ToString( )
               } ).CliConfigObject;

         ClassicAssert.AreEqual( int.MinValue, cli.Arg );
      }

      [Test]
      public void IntArg_Zero( ) {
         ICliIntArg cli = new InterfaceSpecBoilerPlateHelper<ICliIntArg>(
               new[] {
                     0.ToString( )
               } ).CliConfigObject;

         ClassicAssert.AreEqual( 0, cli.Arg );
      }

      [Test]
      public void LongArg_MaxValue( ) {
         ICliLongArg cli = new InterfaceSpecBoilerPlateHelper<ICliLongArg>(
               new[] {
                     long.MaxValue.ToString( )
               } ).CliConfigObject;

         ClassicAssert.AreEqual( long.MaxValue, cli.Arg );
      }

      [Test]
      public void LongArg_MinValue( ) {
         ICliLongArg cli = new InterfaceSpecBoilerPlateHelper<ICliLongArg>(
               new[] {
                     long.MinValue.ToString( )
               } ).CliConfigObject;

         ClassicAssert.AreEqual( long.MinValue, cli.Arg );
      }

      [Test]
      public void LongArg_Zero( ) {
         ICliLongArg cli = new InterfaceSpecBoilerPlateHelper<ICliLongArg>(
               new[] {
                     0.ToString( )
               } ).CliConfigObject;

         ClassicAssert.AreEqual( 0, cli.Arg );
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

         ClassicAssert.AreEqual( "a string", cli.StringOptionOne );
         ClassicAssert.AreEqual( "another string", cli.StringOptionTwo );
         ClassicAssert.AreEqual( 42, cli.StringOptionTwoCount );
         ClassicAssert.AreEqual( true, cli.MyFlag );
         ClassicAssert.AreEqual( 3, cli.StringOptionOneDynIntArg.Count( ) );
         ClassicAssert.AreEqual( 1, cli.StringOptionOneDynIntArg.First( ) );
         ClassicAssert.AreEqual( 3, cli.StringOptionOneDynIntArg.Last( ) );
      }

      [Test]
      public void NullableEnumArg_IsNotSet( ) {
         ICliNullableEnumArg cli = new InterfaceSpecBoilerPlateHelper<ICliNullableEnumArg>(
               new string[0] ).CliConfigObject;

         ClassicAssert.IsFalse( cli.Arg.HasValue );
      }

      [Test]
      public void NullableEnumArg_IsSet( ) {
         ICliNullableEnumArg cli = new InterfaceSpecBoilerPlateHelper<ICliNullableEnumArg>(
               new[] {
                     MyEnum.Value2.ToString( )
               } ).CliConfigObject;

         ClassicAssert.AreEqual( MyEnum.Value2, cli.Arg );
      }

      [Test]
      public void NullableInt_NotSet( ) {
         ICliNullableInt cli = new InterfaceSpecBoilerPlateHelper<ICliNullableInt>(
               new string[0] ).CliConfigObject;

         ClassicAssert.IsFalse( cli.Arg.HasValue );
      }

      [Test]
      public void NullableInt_Set( ) {
         ICliNullableInt cli = new InterfaceSpecBoilerPlateHelper<ICliNullableInt>(
               new[] {
                     42.ToString( )
               } ).CliConfigObject;

         ClassicAssert.IsTrue( cli.Arg.HasValue );
         ClassicAssert.AreEqual( 42, cli.Arg );
      }

   }

}
