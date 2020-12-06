// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.IO;

using NUnit.Framework;

namespace jsc.commons.cli.tests.arguments {

   [TestFixture]
   public class FileSystemArgumentsTestsBase {

      [SetUp]
      public void SetUp( ) {
         CleanUp( );
         __tempBaseDirDI.Create( );
         __tempBaseDirDI.Refresh( );
      }

      [TearDown]
      public void TearDown( ) {
         CleanUp( );
      }

      private const string __tempBaseDir = "01aa4596-5919-461b-ac73-ddd77b984c41";
      protected static readonly DirectoryInfo __tempBaseDirDI;

      static FileSystemArgumentsTestsBase( ) {
         __tempBaseDirDI = new DirectoryInfo( Path.Combine( Path.GetTempPath( ), __tempBaseDir ) );
      }

      private static void CleanUp( ) {
         __tempBaseDirDI.Refresh( );
         if( !__tempBaseDirDI.Exists )
            return;

         Directory.Delete( __tempBaseDirDI.FullName, true );
         __tempBaseDirDI.Refresh( );
      }

      [Test]
      [Order( int.MinValue )]
      public void TestSetupWorks( ) {
         Assert.IsTrue( __tempBaseDirDI.Exists );
      }

   }

}