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
using jsc.commons.cli.ispec.constraints.attrib;
using jsc.commons.cli.ispec.constraints.interfaces;
using jsc.commons.config.interfaces;
using jsc.commons.rc.interfaces;

using NUnit.Framework;

namespace jsc.commons.cli.tests.ispec.constraints {

   [TestFixture]
   public class ArgumentResolving {

      private const int Value = 42;

      [CliDefinition]
      [ConstraintsProvider( typeof( Cli1ConstraintsProvider ) )]
      public interface ICli1 : IConfiguration {

         [Argument]
         int Arg { get; set; }

      }

      public class Cli1ConstraintsProvider : IConstraintsProvider<ICli1> {

         public void ProvideConstraints( IConstraintsProviderContext<ICli1> cpc ) {
            cpc.Add( cpc.ArgumentIsSet( nameof( ICli1.Arg ) ) );
         }

      }

      [CliDefinition]
      [ConstraintsProvider( typeof( Cli2ConstraintsProvider ) )]
      public interface ICli2 : IConfiguration {

         [Argument( Of = nameof( Option ) )]
         int Arg { get; set; }

         [Option( Flag = 'o' )]
         bool Option { get; set; }

      }

      public class Cli2ConstraintsProvider : IConstraintsProvider<ICli2> {

         public void ProvideConstraints( IConstraintsProviderContext<ICli2> cpc ) {
            cpc.Add( cpc.ArgumentIsSet( nameof( ICli2.Arg ) ) );
         }

      }

      [CliDefinition]
      [ConstraintsProvider( typeof( Cli3ConstraintsProvider ) )]
      public interface ICli3 : IConfiguration {

         [Option( Flag = 'o' )]
         int Option { get; set; }

      }

      public class Cli3ConstraintsProvider : IConstraintsProvider<ICli3> {

         public void ProvideConstraints( IConstraintsProviderContext<ICli3> cpc ) {
            cpc.Add( cpc.ArgumentIsSet( nameof( ICli3.Option ) ) );
         }

      }

      [CliDefinition]
      [ConstraintsProvider( typeof( Cli4ConstraintsProvider ) )]
      public interface ICli4 : IConfiguration {

         [Option( Flag = 'o' )]
         [FirstArgument( Name = "IntArg" )]
         int Option { get; set; }

      }

      public class Cli4ConstraintsProvider : IConstraintsProvider<ICli4> {

         public void ProvideConstraints( IConstraintsProviderContext<ICli4> cpc ) {
            cpc.Add( cpc.ArgumentIsSet( nameof( ICli4.Option ) ) );
         }

      }

      [Test]
      public void ResolveArg( ) {
         InterfaceSpecBoilerPlateHelper<ICli1> isbph =
               new InterfaceSpecBoilerPlateHelper<ICli1>(
                     new[] {
                           Value.ToString( )
                     } );

         List<ISolution<IParserResult>> solutions = null;
         isbph.Config.UserPrompt = list => {
            solutions = list.ToList( );
            return list.First( );
         };

         ICli1 cli = isbph.CliConfigObject;

         Assert.IsNull( solutions );
         Assert.AreEqual( Value, cli.Arg );
      }

      [Test]
      public void ResolveOptionArg( ) {
         InterfaceSpecBoilerPlateHelper<ICli2> isbph =
               new InterfaceSpecBoilerPlateHelper<ICli2>(
                     new[] {
                           "-o",
                           Value.ToString( )
                     } );

         List<ISolution<IParserResult>> solutions = null;
         isbph.Config.UserPrompt = list => {
            solutions = list.ToList( );
            return list.First( );
         };

         ICli2 cli = isbph.CliConfigObject;

         Assert.IsNull( solutions );
         Assert.AreEqual( Value, cli.Arg );
      }

      [Test]
      public void ResolveOptionImplicitArg( ) {
         InterfaceSpecBoilerPlateHelper<ICli3> isbph =
               new InterfaceSpecBoilerPlateHelper<ICli3>(
                     new[] {
                           "-o",
                           Value.ToString( )
                     } );

         List<ISolution<IParserResult>> solutions = null;
         isbph.Config.UserPrompt = list => {
            solutions = list.ToList( );
            return list.First( );
         };

         ICli3 cli = isbph.CliConfigObject;

         Assert.IsNull( solutions );
         Assert.AreEqual( Value, cli.Option );
      }

      [Test]
      public void ResolveOptionImplicitArgWithExplicitName( ) {
         InterfaceSpecBoilerPlateHelper<ICli4> isbph =
               new InterfaceSpecBoilerPlateHelper<ICli4>(
                     new[] {
                           "-o",
                           Value.ToString( )
                     } );

         List<ISolution<IParserResult>> solutions = null;
         isbph.Config.UserPrompt = list => {
            solutions = list.ToList( );
            return list.First( );
         };

         ICli4 cli = isbph.CliConfigObject;

         Assert.IsNull( solutions );
         Assert.AreEqual( Value, cli.Option );
      }

   }

}