// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using jsc.commons.cli.arguments;
using jsc.commons.cli.interfaces;
using jsc.commons.cli.ispec;
using jsc.commons.cli.ispec.attrib;
using jsc.commons.cli.ispec.constraints.attrib;
using jsc.commons.cli.ispec.constraints.interfaces;
using jsc.commons.cli.rules;
using jsc.commons.config.interfaces;
using jsc.commons.rc.generic;
using jsc.commons.rc.generic.rules;
using jsc.commons.rc.interfaces;

using NUnit.Framework;

namespace jsc.commons.cli.tests.arguments {

   [TestFixture]
   public class FileArgumentTests : FileSystemArgumentsTestsBase {

      [Test]
      public void ArgDerivedAsFileInfoArg( ) {
         InterfaceSpecBoilerPlateHelper<ICli> isbph = new InterfaceSpecBoilerPlateHelper<ICli>( new string[0] );
         IArgument arg = isbph.CliSpecification.Arguments.FirstOrDefault( );
         Assert.IsNotNull( arg );
         Assert.IsInstanceOf<FileArgument>( arg );
      }

      [Test]
      public void FileArgIsSet( ) {
         InterfaceSpecBoilerPlateHelper<ICli> isbph = new InterfaceSpecBoilerPlateHelper<ICli>(
               new[] {
                     Path.Combine( "a", "s", "d.f" )
               } );
         ICli cli = isbph.CliConfigObject;
         Assert.IsNotNull( cli.FileArg );
         Assert.AreEqual( new DirectoryInfo( "a/s" ), cli.FileArg.Directory );
         Assert.AreEqual( "d.f", cli.FileArg.Name );
      }

      [Test]
      public void FileArgDoesNotExist( ) {
         InterfaceSpecBoilerPlateHelper<ICli2> isbph = new InterfaceSpecBoilerPlateHelper<ICli2>(
               new[] {
                     Path.Combine(
                           __tempBaseDirDI.FullName,
                           "as.df" )
               } );

         IList<ISolution<IParserResult>> solutions = null;
         isbph.Config.UserPrompt = solutions2 => {
            solutions = solutions2;
            return solutions2.FirstOrDefault(
                  solution => solution.Actions.Any(
                        action => action is GenericAction<IParserResult> genericAction
                              &&genericAction.Description.Equals( "cancel" ) ) );
         };

         Exception exc = null;
         try {
            ICli2 unused = isbph.CliConfigObject;
         } catch( Exception exc2 ) {
            exc = exc2;
         }

         Assert.IsNotNull( solutions );
         Assert.IsNotNull( exc );
         Assert.AreEqual( "unresolved conflicts", exc.Message );
      }

      [Test]
      public void FileArgExists( ) {
         FileInfo fi = new FileInfo(
               Path.Combine(
                     __tempBaseDirDI.FullName,
                     "qw.ert" ) );

         try {
            fi.Create( );
            InterfaceSpecBoilerPlateHelper<ICli2> isbph = new InterfaceSpecBoilerPlateHelper<ICli2>(
                  new[] {
                        Path.Combine(
                              __tempBaseDirDI.FullName,
                              "qw.ert" )
                  } );

            IList<ISolution<IParserResult>> solutions = null;
            isbph.Config.UserPrompt = solutions2 => {
               solutions = solutions2;
               return solutions2.FirstOrDefault(
                     solution => solution.Actions.Any(
                           action => action is GenericAction<IParserResult> genericAction
                                 &&genericAction.Description.Equals( "cancel" ) ) );
            };

            Exception exc = null;
            ICli2 cli = null;
            try {
               cli = isbph.CliConfigObject;
            } catch( Exception exc2 ) {
               exc = exc2;
            }

            Assert.IsNull( solutions );
            Assert.IsNull( exc );
            Assert.IsNotNull( cli );
            Assert.IsNotNull( cli.FileArg );
            Assert.IsTrue( cli.FileArg.Exists );
         } finally {
            fi.Delete( );
         }
      }

   }

   [CliDefinition]
   public interface ICli : IConfiguration {

      [Argument]
      FileInfo FileArg { get; set; }

   }

   [ConstraintsProvider( typeof( Cli2Cp ) )]
   [CliDefinition]
   public interface ICli2 : IConfiguration {

      [Argument]
      FileInfo FileArg { get; set; }

   }

   internal class Cli2Cp : IConstraintsProvider<ICli2> {

      public void ProvideConstraints( IConstraintsProviderContext<ICli2> cpc ) {
         cpc.Add(
               new Implies<IParserResult>(
                     cpc.ArgumentIsSet( nameof( ICli2.FileArg ) ),
                     new FileExists( (FileSystemItemArgument)cpc.GetArgument( nameof( ICli2.FileArg ) ) ) ) );
      }

   }

}