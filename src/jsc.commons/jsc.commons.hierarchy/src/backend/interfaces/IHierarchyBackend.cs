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

namespace jsc.commons.hierarchy.backend.interfaces {

   public interface IHierarchyBackend : IDisposable {

      Task<IResource> Get( IPath path );

      Task<IEnumerable<string>> List( IPath path );

      Task Set( IResource resource );

      Task Delete( IResource resource );

   }

}