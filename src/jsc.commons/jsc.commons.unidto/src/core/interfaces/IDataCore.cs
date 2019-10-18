// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

namespace jsc.commons.unidto.core.interfaces {

   public interface IDataCore {

      Type Type { get; }

      bool IsNull( string key );

      object Get( string key );

      void Set( string key, object value );

      object[] GetData( );

   }

}