// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.misc;

namespace jsc.commons.hierarchy {

   public static class HierarchyExtensions {

      public static T Get<T>( this IHierarchy hierarchy, IPath path, string resourceName ) where T : IResource {
         hierarchy.MustNotBeNull( nameof( hierarchy ) );

         return hierarchy.Get<T>( path.Append( resourceName ) );
      }

      public static T Get<T>( this IHierarchy hierarchy, string path ) where T : IResource {
         hierarchy.MustNotBeNull( nameof( hierarchy ) );
         path.MustNotBeNull( nameof( path ) );

         return hierarchy.Get<T>( Path.Parse( path ) );
      }

      public static T Get<T>( this IHierarchy hierarchy, string path, string resourceName ) where T : IResource {
         hierarchy.MustNotBeNull( nameof( hierarchy ) );
         path.MustNotBeNull( nameof( path ) );
         resourceName.MustNotBeNull( nameof( resourceName ) );

         return hierarchy.Get<T>( Path.Parse( path ).Append( resourceName ) );
      }

      public static bool TryGet<T>(
            this IHierarchy hierarchy,
            IPath path,
            string resourceName,
            out IResource resource ) where T : IResource {
         hierarchy.MustNotBeNull( nameof( hierarchy ) );
         path.MustNotBeNull( nameof( path ) );
         resourceName.MustNotBeNull( nameof( resourceName ) );

         return hierarchy.TryGet( path.Append( resourceName ), out resource );
      }

      public static bool TryGet<T>(
            this IHierarchy hierarchy,
            string path,
            out IResource resource ) where T : IResource {
         hierarchy.MustNotBeNull( nameof( hierarchy ) );
         path.MustNotBeNull( nameof( path ) );

         return hierarchy.TryGet( Path.Parse( path ), out resource );
      }

      public static bool TryGet<T>(
            this IHierarchy hierarchy,
            string path,
            string resourceName,
            out IResource resource ) where T : IResource {
         hierarchy.MustNotBeNull( nameof( hierarchy ) );
         path.MustNotBeNull( nameof( path ) );
         resourceName.MustNotBeNull( nameof( resourceName ) );

         return hierarchy.TryGet( Path.Parse( path ).Append( resourceName ), out resource );
      }

   }

}
