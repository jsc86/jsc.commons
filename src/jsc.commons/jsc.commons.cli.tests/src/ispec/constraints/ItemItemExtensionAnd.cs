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

namespace jsc.commons.cli.tests.ispec.constraints {

   [TestFixture]
   public class ItemItemExtensionAnd {

      [CliDefinition]
      [ConstraintsProvider( typeof( CliAndConstraintsProvider ) )]
      public interface ICliAnd : IConfiguration {

         [Option( Flag = 'o' )]
         bool Option { get; set; }

         [Flag( Name = 'f' )]
         bool Flag { get; set; }

      }

      public class CliAndConstraintsProvider : IConstraintsProvider<ICliAnd> {

         public void ProvideConstraints( IConstraintsProviderContext<ICliAnd> cpc ) {
            cpc.Add( cpc.AndItemItem( nameof( ICliAnd.Option ), nameof( ICliAnd.Flag ) ) );
         }

      }

      [Test]
      public void ConstraintAndFalseFalse( ) {
         InterfaceSpecBoilerPlateHelper<ICliAnd> isbph =
               new InterfaceSpecBoilerPlateHelper<ICliAnd>(
                     new string[0] );

         List<ISolution<IParserResult>> solutions = null;

         isbph.Config.UserPrompt = list => {
            solutions = list.ToList( );
            return list.First( );
         };

         ICliAnd cli = isbph.CliConfigObject;

         Assert.IsNotNull( solutions );
         Assert.IsTrue( cli.Flag&&cli.Option );
      }

      [Test]
      public void ConstraintAndFalseTrue( ) {
         InterfaceSpecBoilerPlateHelper<ICliAnd> isbph =
               new InterfaceSpecBoilerPlateHelper<ICliAnd>(
                     new[] {"-f"} );

         List<ISolution<IParserResult>> solutions = null;

         isbph.Config.UserPrompt = list => {
            solutions = list.ToList( );
            return list.First( );
         };

         ICliAnd cli = isbph.CliConfigObject;

         Assert.IsNotNull( solutions );
         Assert.IsTrue( cli.Flag&&cli.Option );
      }

      [Test]
      public void ConstraintAndTrueFalse( ) {
         InterfaceSpecBoilerPlateHelper<ICliAnd> isbph =
               new InterfaceSpecBoilerPlateHelper<ICliAnd>(
                     new[] {"-o"} );

         List<ISolution<IParserResult>> solutions = null;

         isbph.Config.UserPrompt = list => {
            solutions = list.ToList( );
            return list.First( );
         };

         ICliAnd cli = isbph.CliConfigObject;

         Assert.IsNotNull( solutions );
         Assert.IsTrue( cli.Flag&&cli.Option );
      }

      [Test]
      public void ConstraintAndTrueTrue( ) {
         InterfaceSpecBoilerPlateHelper<ICliAnd> isbph =
               new InterfaceSpecBoilerPlateHelper<ICliAnd>(
                     new[] {"-of"} );

         List<ISolution<IParserResult>> solutions = null;

         isbph.Config.UserPrompt = list => {
            solutions = list.ToList( );
            return list.First( );
         };

         ICliAnd cli = isbph.CliConfigObject;

         Assert.IsNull( solutions );
         Assert.IsTrue( cli.Flag&&cli.Option );
      }

   }

}