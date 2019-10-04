// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.unidto.core.attributes;

namespace jsc.commons.unidto.core.interfaces {

   [Marker( nameof( IVersioned ) )]
   [Implementation( typeof( VersionedDataCore ) )]
   public interface IVersioned : IDataCore {

      uint CurrentVersion { get; }

      void ResetToVersion( uint version );

      void ResetToSquashed( );

      void SquashChanges( );

      bool HasChanges( );

   }

}
