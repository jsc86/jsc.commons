// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Threading.Tasks;

using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.misc;

namespace jsc.commons.hierarchy {

   public static class HierarchyAsyncExtensions {

      public static async Task<T> GetAsync<T>(
            this IHierarchyAsync hierarchyAsync,
            Path path,
            string resourceName ) where T : IResource {
         hierarchyAsync.MustNotBeNull( nameof( HierarchyAsync ) );

         return await hierarchyAsync.GetAsync<T>( path.Append( resourceName ) );
      }

      public static async Task<T> GetAsync<T>(
            this IHierarchyAsync hierarchyAsync,
            string path ) where T : IResource {
         hierarchyAsync.MustNotBeNull( nameof( hierarchyAsync ) );
         path.MustNotBeNull( nameof( path ) );

         return await hierarchyAsync.GetAsync<T>( Path.Parse( path ) );
      }

      public static async Task<T> GetAsync<T>(
            this IHierarchyAsync hierarchyAsync,
            string path,
            string resourceName ) where T : IResource {
         hierarchyAsync.MustNotBeNull( nameof( hierarchyAsync ) );
         path.MustNotBeNull( nameof( path ) );
         resourceName.MustNotBeNull( nameof( resourceName ) );

         return await hierarchyAsync.GetAsync<T>( Path.Parse( path ).Append( resourceName ) );
      }

   }

}
