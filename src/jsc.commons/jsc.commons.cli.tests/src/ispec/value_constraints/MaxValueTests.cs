// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Linq;

using jsc.commons.cli.arguments;
using jsc.commons.cli.interfaces;
using jsc.commons.cli.ispec;
using jsc.commons.cli.ispec.attrib;
using jsc.commons.config.interfaces;
using jsc.commons.rc.interfaces;

using NUnit.Framework;

namespace jsc.commons.cli.tests.ispec.value_constraints {

   [TestFixture]
   public class MaxValueTests {

      private const int IntMaxVal = -42;
      private const long LongMaxVal = -42;
      private const float FloatMaxVal = -42;
      private const double DoubleMaxVal = -42;

      [CliDefinition]
      public interface ICliIntArg : IConfiguration {

         [Argument]
         [IntArg.MaxValueAttribute( IntMaxVal )]
         int IntArg { get; set; }

      }

      [CliDefinition]
      public interface ICliLongArg : IConfiguration {

         [Argument]
         [LongArg.MaxValueAttribute( LongMaxVal )]
         long LongArg { get; set; }

      }

      [CliDefinition]
      public interface ICliFloatArg : IConfiguration {

         [Argument]
         [FloatArg.MaxValueAttribute( FloatMaxVal )]
         float FloatArg { get; set; }

      }

      [CliDefinition]
      public interface ICliDoubleArg : IConfiguration {

         [Argument]
         [DoubleArg.MaxValueAttribute( DoubleMaxVal )]
         double DoubleArg { get; set; }

      }

      private static ISolution<IParserResult> AutoSolve( IEnumerable<ISolution<IParserResult>> solutions ) {
         return solutions.FirstOrDefault( solution => solution.Description.Contains( "auto solve" ) );
      }

      [Test]
      public void DoubleArgMinVal( ) {
         InterfaceSpecBoilerPlateHelper<ICliDoubleArg> isbph =
               new InterfaceSpecBoilerPlateHelper<ICliDoubleArg>(
                     new[] {0.ToString( )} );

         isbph.Config.UserPrompt = AutoSolve;

         Assert.AreEqual( DoubleMaxVal, isbph.CliConfigObject.DoubleArg );
      }

      [Test]
      public void FloatArgMinVal( ) {
         InterfaceSpecBoilerPlateHelper<ICliFloatArg> isbph =
               new InterfaceSpecBoilerPlateHelper<ICliFloatArg>(
                     new[] {0.ToString( )} );

         isbph.Config.UserPrompt = AutoSolve;

         Assert.AreEqual( FloatMaxVal, isbph.CliConfigObject.FloatArg );
      }

      [Test]
      public void IntArgMinVal( ) {
         InterfaceSpecBoilerPlateHelper<ICliIntArg> isbph =
               new InterfaceSpecBoilerPlateHelper<ICliIntArg>(
                     new[] {0.ToString( )} );

         isbph.Config.UserPrompt = AutoSolve;

         Assert.AreEqual( IntMaxVal, isbph.CliConfigObject.IntArg );
      }

      [Test]
      public void LongArgMinVal( ) {
         InterfaceSpecBoilerPlateHelper<ICliLongArg> isbph =
               new InterfaceSpecBoilerPlateHelper<ICliLongArg>(
                     new[] {0.ToString( )} );

         isbph.Config.UserPrompt = AutoSolve;

         Assert.AreEqual( LongMaxVal, isbph.CliConfigObject.LongArg );
      }

   }

}