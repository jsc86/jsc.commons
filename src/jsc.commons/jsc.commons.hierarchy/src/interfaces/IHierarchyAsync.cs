// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using jsc.commons.hierarchy.config;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;

namespace jsc.commons.hierarchy.interfaces {

   public interface IHierarchyAsync : IDisposable {

      IHierarchyConfiguration Configuration { get; }

      Task<T> GetAsync<T>( IPath path ) where T : IResource;

      Task SetAsync( IResource resource );

      Task DeleteAsync( IResource resource );

      Task<IEnumerable<string>> GetChildrenResourceNamesAsync( IPath path );

      Task MoveAsync( IResource resource, IPath targetPath );

      event ResourceSetHandler ResourceCreated;

      event ResourceDeletedHandler ResourceDeleted;

      event ResourceMovedHandler ResourceMoved;

   }

}
