// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.resources.interfaces;

namespace jsc.commons.hierarchy {

   public class HierarchyEventHull : HierarchyAsyncEventHull, IHierarchy {

      private readonly IHierarchy _hierarchy;

      public HierarchyEventHull( IHierarchy hierarchy ) : base( hierarchy ) {
         _hierarchy = hierarchy;
      }

      public T Get<T>( Path path ) where T : IResource {
         return _hierarchy.Get<T>( path );
      }

      public bool TryGet<T>( Path path, out T resource ) where T : IResource {
         return _hierarchy.TryGet( path, out resource );
      }

      public void Set( IResource resource ) {
         _hierarchy.Set( resource );
      }

      public void Delete( IResource resource ) {
         _hierarchy.Delete( resource );
      }

      public IEnumerable<string> GetChildrenResourceNames( Path path ) {
         return _hierarchy.GetChildrenResourceNames( path );
      }

      public void Move( IResource resource, Path targetPath ) {
         _hierarchy.Move( resource, targetPath );
      }

      public TimeSpan Timeout => _hierarchy.Timeout;

   }

}
