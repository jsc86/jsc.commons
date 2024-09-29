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
   public class ItemItemExtensionImplies {

      [CliDefinition]
      [ConstraintsProvider( typeof( CliImpliesConstraintsProvider ) )]
      public interface ICliImplies : IConfiguration {

         [Option( Flag = 'o' )]
         bool Option { get; set; }

         [Flag( Name = 'f' )]
         bool Flag { get; set; }

      }

      public class CliImpliesConstraintsProvider : IConstraintsProvider<ICliImplies> {

         public void ProvideConstraints( IConstraintsProviderContext<ICliImplies> cpc ) {
            cpc.Add( cpc.ImpliesItemItem( nameof( ICliImplies.Option ), nameof( ICliImplies.Flag ) ) );
         }

      }

      [Test]
      public void ConstraintImpliesFalseFalse( ) {
         InterfaceSpecBoilerPlateHelper<ICliImplies> isbph =
               new InterfaceSpecBoilerPlateHelper<ICliImplies>(
                     new string[0] );

         List<ISolution<IParserResult>> solutions = null;

         isbph.Config.UserPrompt = list => {
            solutions = list.ToList( );
            return list.First( );
         };

         ICliImplies cli = isbph.CliConfigObject;

         ClassicAssert.IsNull( solutions );
         ClassicAssert.IsFalse( cli.Flag||cli.Option );
      }

      [Test]
      public void ConstraintImpliesFalseTrue( ) {
         InterfaceSpecBoilerPlateHelper<ICliImplies> isbph =
               new InterfaceSpecBoilerPlateHelper<ICliImplies>(
                     new[] {"-f"} );

         List<ISolution<IParserResult>> solutions = null;

         isbph.Config.UserPrompt = list => {
            solutions = list.ToList( );
            return list.First( );
         };

         ICliImplies cli = isbph.CliConfigObject;

         ClassicAssert.IsNull( solutions );
         ClassicAssert.IsTrue( cli.Flag );
         ClassicAssert.IsFalse( cli.Option );
      }

      [Test]
      public void ConstraintImpliesTrueFalse( ) {
         InterfaceSpecBoilerPlateHelper<ICliImplies> isbph =
               new InterfaceSpecBoilerPlateHelper<ICliImplies>(
                     new[] {"-o"} );

         List<ISolution<IParserResult>> solutions = null;

         isbph.Config.UserPrompt = list => {
            solutions = list.ToList( );
            return list.First( );
         };

         ICliImplies cli = isbph.CliConfigObject;

         ClassicAssert.IsNotNull( solutions );
         ClassicAssert.IsTrue( cli.Flag&&cli.Option );
      }

      [Test]
      public void ConstraintImpliesTrueTrue( ) {
         InterfaceSpecBoilerPlateHelper<ICliImplies> isbph =
               new InterfaceSpecBoilerPlateHelper<ICliImplies>(
                     new[] {"-of"} );

         List<ISolution<IParserResult>> solutions = null;

         isbph.Config.UserPrompt = list => {
            solutions = list.ToList( );
            return list.First( );
         };

         ICliImplies cli = isbph.CliConfigObject;

         ClassicAssert.IsNull( solutions );
         ClassicAssert.IsTrue( cli.Flag||cli.Option );
      }

   }

}
