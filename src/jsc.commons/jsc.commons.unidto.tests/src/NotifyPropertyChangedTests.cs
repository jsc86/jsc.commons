// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Reflection;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace jsc.commons.unidto.tests {

   [TestFixture]
   public class NotifyPropertyChangedTests {

      private class Change {

         public Change( string propertyName, object value ) {
            PropertyName = propertyName;
            Value = value;
         }

         public string PropertyName { get; }
         public object Value { get; }

         public override string ToString( ) {
            return $"{PropertyName}: {Value}";
         }

      }

      private static Change GetChange( object dto, string properName ) {
         return new Change(
               properName,
               // ReSharper disable once PossibleNullReferenceException
               dto.GetType( )
                     .GetProperty(
                           properName,
                           BindingFlags.Public|BindingFlags.Instance|BindingFlags.FlattenHierarchy )
                     .GetMethod.Invoke( dto, Array.Empty<object>( ) ) );
      }

      [Test]
      public void AdvancedDataCore_AcceptChanges( ) {
         Queue<Change> changes = new Queue<Change>( );
         ITestInterfaceAdcNpc dto = DtoCreator.New<ITestInterfaceAdcNpc>( );
         dto.PropertyChanged += ( sender, args ) => {
            changes.Enqueue(
                  GetChange( dto, args.PropertyName ) );
         };

         dto.Prop = 42;
         dto.Prop = 23;
         dto.AcceptChanges( );

         ClassicAssert.AreEqual( 4, changes.Count );

         Change change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceAdcNpc.HasChanges ), change.PropertyName );
         ClassicAssert.AreEqual( true, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceAdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( 42, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceAdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( 23, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceAdcNpc.HasChanges ), change.PropertyName );
         ClassicAssert.AreEqual( false, change.Value );
      }

      [Test]
      public void AdvancedDataCore_RevertChanges( ) {
         Queue<Change> changes = new Queue<Change>( );
         ITestInterfaceAdcNpc dto = DtoCreator.New<ITestInterfaceAdcNpc>( );
         dto.PropertyChanged += ( sender, args ) => {
            changes.Enqueue(
                  GetChange( dto, args.PropertyName ) );
         };

         dto.Prop = 42;
         dto.Prop = 23;
         dto.RevertChanges( );

         ClassicAssert.AreEqual( 5, changes.Count );

         Change change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceAdcNpc.HasChanges ), change.PropertyName );
         ClassicAssert.AreEqual( true, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceAdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( 42, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceAdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( 23, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceAdcNpc.HasChanges ), change.PropertyName );
         ClassicAssert.AreEqual( false, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceAdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( null, change.Value );
      }

      [Test]
      public void AdvancedDataCore_RevertToAcceptedChanges( ) {
         Queue<Change> changes = new Queue<Change>( );
         ITestInterfaceAdcNpc dto = DtoCreator.New<ITestInterfaceAdcNpc>( );
         dto.PropertyChanged += ( sender, args ) => {
            changes.Enqueue(
                  GetChange( dto, args.PropertyName ) );
         };

         dto.Prop = 42;
         dto.AcceptChanges( );
         dto.Prop = 23;
         dto.RevertChanges( );

         ClassicAssert.AreEqual( 7, changes.Count );

         Change change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceAdcNpc.HasChanges ), change.PropertyName );
         ClassicAssert.AreEqual( true, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceAdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( 42, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceAdcNpc.HasChanges ), change.PropertyName );
         ClassicAssert.AreEqual( false, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceAdcNpc.HasChanges ), change.PropertyName );
         ClassicAssert.AreEqual( true, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceAdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( 23, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceAdcNpc.HasChanges ), change.PropertyName );
         ClassicAssert.AreEqual( false, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceAdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( 42, change.Value );
      }

      [Test]
      public void SimpleDataCore( ) {
         Queue<Change> changes = new Queue<Change>( );
         ITestInterfaceSdcNpc dto = DtoCreator.New<ITestInterfaceSdcNpc>( );
         dto.PropertyChanged += ( sender, args ) => {
            changes.Enqueue(
                  GetChange( dto, args.PropertyName ) );
         };

         dto.Prop = 42;
         dto.Prop = 23;
         dto.MarkNotDirty( );

         ClassicAssert.AreEqual( 4, changes.Count );

         Change change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceSdcNpc.IsDirty ), change.PropertyName );
         ClassicAssert.AreEqual( true, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceSdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( 42, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceSdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( 23, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceSdcNpc.IsDirty ), change.PropertyName );
         ClassicAssert.AreEqual( false, change.Value );
      }

      [Test]
      public void VersionedDataCore_ResetToSquashed( ) {
         Queue<Change> changes = new Queue<Change>( );
         ITestInterfaceVdcNpc dto = DtoCreator.New<ITestInterfaceVdcNpc>( );
         dto.PropertyChanged += ( sender, args ) => {
            changes.Enqueue(
                  GetChange( dto, args.PropertyName ) );
         };

         dto.Prop = 42;
         dto.Prop = 23;
         dto.ResetToSquashed( );

         ClassicAssert.AreEqual( 8, changes.Count );

         Change change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.CurrentVersion ), change.PropertyName );
         ClassicAssert.AreEqual( 1, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.HasChanges ), change.PropertyName );
         ClassicAssert.AreEqual( true, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( 42, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.CurrentVersion ), change.PropertyName );
         ClassicAssert.AreEqual( 2, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( 23, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.CurrentVersion ), change.PropertyName );
         ClassicAssert.AreEqual( 0, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.HasChanges ), change.PropertyName );
         ClassicAssert.AreEqual( false, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( null, change.Value );
      }

      [Test]
      public void VersionedDataCore_ResetToVersion( ) {
         Queue<Change> changes = new Queue<Change>( );
         ITestInterfaceVdcNpc dto = DtoCreator.New<ITestInterfaceVdcNpc>( );
         dto.PropertyChanged += ( sender, args ) => {
            changes.Enqueue(
                  GetChange( dto, args.PropertyName ) );
         };

         dto.Prop = 42;
         dto.Prop = 23;
         dto.ResetToVersion( 1 );

         ClassicAssert.AreEqual( 7, changes.Count );

         Change change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.CurrentVersion ), change.PropertyName );
         ClassicAssert.AreEqual( 1, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.HasChanges ), change.PropertyName );
         ClassicAssert.AreEqual( true, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( 42, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.CurrentVersion ), change.PropertyName );
         ClassicAssert.AreEqual( 2, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( 23, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.CurrentVersion ), change.PropertyName );
         ClassicAssert.AreEqual( 1, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( 42, change.Value );
      }

      [Test]
      public void VersionedDataCore_SquashChanges( ) {
         Queue<Change> changes = new Queue<Change>( );
         ITestInterfaceVdcNpc dto = DtoCreator.New<ITestInterfaceVdcNpc>( );
         dto.PropertyChanged += ( sender, args ) => {
            changes.Enqueue(
                  GetChange( dto, args.PropertyName ) );
         };

         dto.Prop = 42;
         dto.Prop = 23;
         dto.SquashChanges( );

         ClassicAssert.AreEqual( 6, changes.Count );

         Change change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.CurrentVersion ), change.PropertyName );
         ClassicAssert.AreEqual( 1, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.HasChanges ), change.PropertyName );
         ClassicAssert.AreEqual( true, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( 42, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.CurrentVersion ), change.PropertyName );
         ClassicAssert.AreEqual( 2, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.Prop ), change.PropertyName );
         ClassicAssert.AreEqual( 23, change.Value );

         change = changes.Dequeue( );
         ClassicAssert.AreEqual( nameof( ITestInterfaceVdcNpc.HasChanges ), change.PropertyName );
         ClassicAssert.AreEqual( false, change.Value );
      }

   }

}
