// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

namespace jsc.commons.config.interfaces {

   /// <summary>
   ///    A Configuration Backend is a means to read and write configuration instances
   ///    from and to any kind of persistence layer: files, data bases etc.
   /// </summary>
   public interface IConfigurationBackend {

      void Save( IConfiguration config );

      void Read( IConfiguration config, bool skipNullValues = false );

      T Read<T>( ) where T : IConfiguration;

   }

}
