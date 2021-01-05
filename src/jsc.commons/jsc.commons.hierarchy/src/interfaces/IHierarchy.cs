// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;

namespace jsc.commons.hierarchy.interfaces {

   public interface IHierarchy : IHierarchyAsync {

      TimeSpan Timeout { get; }

      T Get<T>( IPath path ) where T : IResource;

      bool TryGet<T>( IPath path, out T resource ) where T : IResource;

      void Set( IResource resource );

      void Delete( IResource resource );

      IEnumerable<string> GetChildrenResourceNames( IPath path );

      void Move( IResource resource, IPath targetPath );

   }

}
