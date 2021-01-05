// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.resources.interfaces;

namespace jsc.commons.hierarchy.interfaces {

   public interface IHierarchy : IHierarchyAsync {

      TimeSpan Timeout { get; }

      T Get<T>( Path path ) where T : IResource;

      bool TryGet<T>( Path path, out T resource ) where T : IResource;

      void Set( IResource resource );

      void Delete( IResource resource );

      IEnumerable<string> GetChildrenResourceNames( Path path );

      void Move( IResource resource, Path targetPath );

   }

}