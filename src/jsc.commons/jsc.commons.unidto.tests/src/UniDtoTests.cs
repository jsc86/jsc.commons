// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.unidto.core.interfaces;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace jsc.commons.unidto.tests {

   [TestFixture( typeof( ITestInterfaceSdc ), typeof( ITestInterfaceAdc ), typeof( ITestInterfaceVdc ) )]
   [TestFixture( typeof( ITestInterfaceSdcNpc ), typeof( ITestInterfaceAdcNpc ), typeof( ITestInterfaceVdcNpc ) )]
   public class UniDtoTests<TSdc, TAdc, TVdc>
         where TSdc : class, ITestInterface, IDirty
         where TAdc : class, ITestInterface, IChangeable
         where TVdc : class, ITestInterface, IVersioned {

      [Test]
      public void AdvancedDataCoreAcceptTest( ) {
         TAdc dto = DtoCreator.New<TAdc>( );

         dto.Prop = 42;
         dto.AcceptChanges( );

         ClassicAssert.AreEqual( 42, dto.Prop );
         ClassicAssert.IsFalse( dto.HasChanges );
      }

      [Test]
      public void AdvancedDataCoreRevertTest( ) {
         TAdc dto = DtoCreator.New<TAdc>( );

         dto.Prop = 42;
         dto.RevertChanges( );

         ClassicAssert.IsFalse( dto.HasChanges );
         ClassicAssert.IsNull( dto.Prop );
      }

      [Test]
      public void AdvancedDataCoreRevertToAcceptedTest( ) {
         TAdc dto = DtoCreator.New<TAdc>( );

         dto.Prop = 42;
         dto.AcceptChanges( );
         dto.Prop = 23;

         ClassicAssert.AreEqual( 23, dto.Prop );
         ClassicAssert.IsTrue( dto.HasChanges );

         dto.RevertChanges( );

         ClassicAssert.AreEqual( 42, dto.Prop );
         ClassicAssert.IsFalse( dto.HasChanges );
      }

      [Test]
      public void AdvancedDataCoreTest( ) {
         TAdc dto = DtoCreator.New<TAdc>( );

         ClassicAssert.IsFalse( dto.HasChanges );
         ClassicAssert.IsNull( dto.Prop );

         dto.Prop = 42;

         ClassicAssert.AreEqual( 42, dto.Prop );
         ClassicAssert.IsTrue( dto.HasChanges );
      }

      [Test]
      public void SimpleDataCoreTest( ) {
         TSdc dto = DtoCreator.New<TSdc>( );

         ClassicAssert.IsFalse( dto.IsDirty );
         ClassicAssert.IsNull( dto.Prop );

         dto.Prop = 42;

         ClassicAssert.AreEqual( 42, dto.Prop );
         ClassicAssert.IsTrue( dto.IsDirty );
      }

      [Test]
      public void VersionedDataCoreResetToSquashedTest( ) {
         TVdc dto = DtoCreator.New<TVdc>( );

         dto.Prop = 1;
         dto.Prop = 42;

         dto.SquashChanges( );

         dto.Prop = 2;
         dto.Prop = 3;

         dto.ResetToSquashed( );

         ClassicAssert.AreEqual( 42, dto.Prop );
         ClassicAssert.IsFalse( dto.HasChanges );
         ClassicAssert.AreEqual( 2, dto.CurrentVersion );
      }

      [Test]
      public void VersionedDataCoreResetToVersionTest( ) {
         TVdc dto = DtoCreator.New<TVdc>( );

         dto.Prop = 1;
         dto.Prop = 42;
         dto.Prop = 2;
         dto.Prop = 3;

         ClassicAssert.AreEqual( 4, dto.CurrentVersion );

         dto.ResetToVersion( 2 );

         ClassicAssert.AreEqual( 42, dto.Prop );
         ClassicAssert.IsTrue( dto.HasChanges );
         ClassicAssert.AreEqual( 2, dto.CurrentVersion );
      }

      [Test]
      public void VersionedDataCoreSquashTest( ) {
         TVdc dto = DtoCreator.New<TVdc>( );

         dto.Prop = 1;
         dto.Prop = 2;
         dto.Prop = 3;
         dto.Prop = 42;

         ClassicAssert.AreEqual( 42, dto.Prop );
         ClassicAssert.IsTrue( dto.HasChanges );
         ClassicAssert.AreEqual( 4, dto.CurrentVersion );

         dto.SquashChanges( );

         ClassicAssert.AreEqual( 42, dto.Prop );
         ClassicAssert.IsFalse( dto.HasChanges );
         ClassicAssert.AreEqual( 4, dto.CurrentVersion );
      }

      [Test]
      public void VersionedDataCoreTest( ) {
         TVdc dto = DtoCreator.New<TVdc>( );

         ClassicAssert.IsNull( dto.Prop );
         ClassicAssert.IsFalse( dto.HasChanges );
         ClassicAssert.AreEqual( 0, dto.CurrentVersion );

         dto.Prop = 42;

         ClassicAssert.AreEqual( 42, dto.Prop );
         ClassicAssert.IsTrue( dto.HasChanges );
         ClassicAssert.AreEqual( 1, dto.CurrentVersion );
      }

   }

}
