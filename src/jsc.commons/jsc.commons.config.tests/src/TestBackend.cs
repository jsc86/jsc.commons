// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

using jsc.commons.config.backend;

namespace jsc.commons.config.tests {

   public class TestBackend : ConfigBackendBase {

      public Dictionary<string, object> Values { get; } = new Dictionary<string, object>( );

      protected override bool Read( string key, Type type, out object value ) {
         return Values.TryGetValue( key, out value );
      }

      protected override void Save( string key, Type type, object value ) {
         Values[ key ] = value;
      }

      protected override void BeforeRead( ) {
         // nop
      }

      protected override void AfterRead( ) {
         // nop
      }

      protected override void OnReadException( Exception exc ) {
         throw exc;
      }

      protected override void BeforeSave( ) {
         // nop
      }

      protected override void AfterSave( ) {
         // nop
      }

      protected override void OnSaveException( Exception exc ) {
         throw exc;
      }

   }

}