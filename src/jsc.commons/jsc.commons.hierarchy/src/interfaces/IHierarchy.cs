// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;

namespace jsc.commons.hierarchy.interfaces {

   public interface IHierarchy : IHierarchyAsync {

      IResource Get( IPath path );

      T Get<T>( IPath path ) where T : IResource;

      bool TryGet( IPath path, out IResource resource );

      bool TryGet<T>( IPath path, out T resource ) where T : IResource;

      void Set( IResource resource );

      void Delete( IResource resource );

      IEnumerable<string> GetChildrenResourceNames( IPath path );

   }

   public interface IHierarchyAsync : IDisposable {

      Task<IResource> GetAsync( IPath path );

      Task<T> GetAsync<T>( IPath path ) where T : IResource;

      Task SetAsync( IResource resource );

      Task DeleteAsync( IResource resource );

      Task<IEnumerable<string>> GetChildrenResourceNamesAsync( IPath path );

   }

}
