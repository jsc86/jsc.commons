// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.unidto.core.interfaces;

using NUnit.Framework;

namespace jsc.commons.unidto.tests {

   [TestFixture]
   public class UniDtoTests {

      [Test]
      public void SimpleDataCoreTest( ) {
         ITestInterfaceSdc dto = DtoCreator.New<ITestInterfaceSdc>( );

         Assert.IsFalse( dto.IsDirty );
         Assert.IsNull( dto.Prop );

         dto.Prop = 42;

         Assert.AreEqual( 42, dto.Prop );
         Assert.IsTrue( dto.IsDirty );
      }

      [Test]
      public void AdvancedDataCoreTest( ) {
         ITestInterfaceAdc dto = DtoCreator.New<ITestInterfaceAdc>( );

         Assert.IsFalse( dto.HasChanges );
         Assert.IsNull( dto.Prop );

         dto.Prop = 42;

         Assert.AreEqual( 42, dto.Prop );
         Assert.IsTrue( dto.HasChanges );
      }

      [Test]
      public void AdvancedDataCoreRevertTest( ) {
         ITestInterfaceAdc dto = DtoCreator.New<ITestInterfaceAdc>( );

         dto.Prop = 42;
         dto.RevertChanges( );

         Assert.IsFalse( dto.HasChanges );
         Assert.IsNull( dto.Prop );
      }

      [Test]
      public void AdvancedDataCoreAcceptTest( ) {
         ITestInterfaceAdc dto = DtoCreator.New<ITestInterfaceAdc>( );

         dto.Prop = 42;
         dto.AcceptChanges( );

         Assert.AreEqual( 42, dto.Prop );
         Assert.IsFalse( dto.HasChanges );
      }

      [Test]
      public void AdvancedDataCoreRevertToAcceptedTest( ) {
         ITestInterfaceAdc dto = DtoCreator.New<ITestInterfaceAdc>( );

         dto.Prop = 42;
         dto.AcceptChanges( );
         dto.Prop = 23;

         Assert.AreEqual( 23, dto.Prop );
         Assert.IsTrue( dto.HasChanges );

         dto.RevertChanges( );

         Assert.AreEqual( 42, dto.Prop );
         Assert.IsFalse( dto.HasChanges );
      }

      [Test]
      public void VersionedDataCoreTest( ) {
         ITestInterfaceVdc dto = DtoCreator.New<ITestInterfaceVdc>( );

         Assert.IsNull( dto.Prop );
         Assert.IsFalse( dto.HasChanges( ) );
         Assert.AreEqual( 0, dto.CurrentVersion );

         dto.Prop = 42;

         Assert.AreEqual( 42, dto.Prop );
         Assert.IsTrue( dto.HasChanges( ) );
         Assert.AreEqual( 1, dto.CurrentVersion );
      }

      [Test]
      public void VersionedDataCoreSquashTest( ) {
         ITestInterfaceVdc dto = DtoCreator.New<ITestInterfaceVdc>( );

         dto.Prop = 1;
         dto.Prop = 2;
         dto.Prop = 3;
         dto.Prop = 42;

         Assert.AreEqual( 42, dto.Prop );
         Assert.IsTrue( dto.HasChanges( ) );
         Assert.AreEqual( 4, dto.CurrentVersion );

         dto.SquashChanges( );

         Assert.AreEqual( 42, dto.Prop );
         Assert.IsFalse( dto.HasChanges( ) );
         Assert.AreEqual( 4, dto.CurrentVersion );
      }

      [Test]
      public void VersionedDataCoreResetToVersionTest( ) {
         ITestInterfaceVdc dto = DtoCreator.New<ITestInterfaceVdc>( );

         dto.Prop = 1;
         dto.Prop = 42;
         dto.Prop = 2;
         dto.Prop = 3;

         Assert.AreEqual( 4, dto.CurrentVersion );

         dto.ResetToVersion( 2 );

         Assert.AreEqual( 42, dto.Prop );
         Assert.IsTrue( dto.HasChanges( ) );
         Assert.AreEqual( 2, dto.CurrentVersion );
      }

      [Test]
      public void VersionedDataCoreResetToSquashedTest( ) {
         ITestInterfaceVdc dto = DtoCreator.New<ITestInterfaceVdc>( );

         dto.Prop = 1;
         dto.Prop = 42;

         dto.SquashChanges( );

         dto.Prop = 2;
         dto.Prop = 3;

         dto.ResetToSquashed( );

         Assert.AreEqual( 42, dto.Prop );
         Assert.IsFalse( dto.HasChanges( ) );
         Assert.AreEqual( 2, dto.CurrentVersion );
      }

   }

   public interface ITestInterfaceSdc : ITestInterface, IDirty { }

   public interface ITestInterfaceAdc : ITestInterface, IChangeable { }

   public interface ITestInterfaceVdc : ITestInterface, IVersioned { }

}
