// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Linq;

using jsc.commons.cli.interfaces;
using jsc.commons.cli.ispec;
using jsc.commons.cli.ispec.attrib;
using jsc.commons.cli.ispec.constraints;
using jsc.commons.cli.ispec.constraints.attrib;
using jsc.commons.cli.ispec.constraints.interfaces;
using jsc.commons.config.interfaces;
using jsc.commons.rc.interfaces;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace jsc.commons.cli.tests.ispec.constraints {

   [TestFixture]
   public class ItemItemExtensionOr {

      [CliDefinition]
      [ConstraintsProvider( typeof( CliOrConstraintsProvider ) )]
      public interface ICliOr : IConfiguration {

         [Option( Flag = 'o' )]
         bool Option { get; set; }

         [Flag( Name = 'f' )]
         bool Flag { get; set; }

      }

      public class CliOrConstraintsProvider : IConstraintsProvider<ICliOr> {

         public void ProvideConstraints( IConstraintsProviderContext<ICliOr> cpc ) {
            cpc.Add( cpc.OrItemItem( nameof( ICliOr.Option ), nameof( ICliOr.Flag ) ) );
         }

      }

      [Test]
      public void ConstraintOrFalseFalse( ) {
         InterfaceSpecBoilerPlateHelper<ICliOr> isbph =
               new InterfaceSpecBoilerPlateHelper<ICliOr>(
                     new string[0] );

         List<ISolution<IParserResult>> solutions = null;

         isbph.Config.UserPrompt = list => {
            solutions = list.ToList( );
            return list.First( );
         };

         ICliOr cli = isbph.CliConfigObject;

         ClassicAssert.IsNotNull( solutions );
         ClassicAssert.IsTrue( cli.Flag||cli.Option );
      }

      [Test]
      public void ConstraintOrFalseTrue( ) {
         InterfaceSpecBoilerPlateHelper<ICliOr> isbph =
               new InterfaceSpecBoilerPlateHelper<ICliOr>(
                     new[] {"-f"} );

         List<ISolution<IParserResult>> solutions = null;

         isbph.Config.UserPrompt = list => {
            solutions = list.ToList( );
            return list.First( );
         };

         ICliOr cli = isbph.CliConfigObject;

         ClassicAssert.IsNull( solutions );
         ClassicAssert.IsTrue( cli.Flag||cli.Option );
      }

      [Test]
      public void ConstraintOrTrueFalse( ) {
         InterfaceSpecBoilerPlateHelper<ICliOr> isbph =
               new InterfaceSpecBoilerPlateHelper<ICliOr>(
                     new[] {"-o"} );

         List<ISolution<IParserResult>> solutions = null;

         isbph.Config.UserPrompt = list => {
            solutions = list.ToList( );
            return list.First( );
         };

         ICliOr cli = isbph.CliConfigObject;

         ClassicAssert.IsNull( solutions );
         ClassicAssert.IsTrue( cli.Flag||cli.Option );
      }

      [Test]
      public void ConstraintOrTrueTrue( ) {
         InterfaceSpecBoilerPlateHelper<ICliOr> isbph =
               new InterfaceSpecBoilerPlateHelper<ICliOr>(
                     new[] {"-of"} );

         List<ISolution<IParserResult>> solutions = null;

         isbph.Config.UserPrompt = list => {
            solutions = list.ToList( );
            return list.First( );
         };

         ICliOr cli = isbph.CliConfigObject;

         ClassicAssert.IsNull( solutions );
         ClassicAssert.IsTrue( cli.Flag||cli.Option );
      }

   }

}
