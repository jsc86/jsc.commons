// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Threading.Tasks;

using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.misc;

namespace jsc.commons.hierarchy {

   public static class IHierarchyAsyncExtensions {

      public static async Task<IResource> Get(
            this IHierarchyAsync hierarchyAsync,
            IPath path,
            string resourceName ) {
         hierarchyAsync.MustNotBeNull( nameof( HierarchyAsync ) );

         return await hierarchyAsync.GetAsync( path.Append( resourceName ) );
      }

      public static async Task<IResource> Get(
            this IHierarchyAsync hierarchyAsync,
            string path ) {
         hierarchyAsync.MustNotBeNull( nameof( hierarchyAsync ) );
         path.MustNotBeNull( nameof( path ) );

         return await hierarchyAsync.GetAsync( Path.Parse( path ) );
      }

      public static async Task<IResource> Get(
            this IHierarchyAsync hierarchyAsync,
            string path,
            string resourceName ) {
         hierarchyAsync.MustNotBeNull( nameof( hierarchyAsync ) );
         path.MustNotBeNull( nameof( path ) );
         resourceName.MustNotBeNull( nameof( resourceName ) );

         return await hierarchyAsync.GetAsync( Path.Parse( path ).Append( resourceName ) );
      }

      public static async Task<T> Get<T, T2>(
            this IHierarchyAsync hierarchyAsync,
            IPath path,
            string resourceName ) where T : IResource<T2> where T2 : IResourceClass {
         return (T)await hierarchyAsync.Get( path, resourceName );
      }

      public static async Task<T> Get<T, T2>(
            this IHierarchyAsync hierarchyAsync,
            string path ) where T : IResource<T2> where T2 : IResourceClass {
         return (T)await hierarchyAsync.Get( path );
      }

      public static async Task<T> Get<T, T2>(
            this IHierarchyAsync hierarchyAsync,
            string path,
            string resourceName ) where T : IResource<T2> where T2 : IResourceClass {
         return (T)await hierarchyAsync.Get( path, resourceName );
      }

   }

}
