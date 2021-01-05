// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Collections.ObjectModel;

using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.path;

namespace jsc.commons.hierarchy.exceptions {

   public class PathOutsideOfBoundsException : HierarchyManagerException {

      public PathOutsideOfBoundsException(
            IHierarchyManagerAsync hierarchyManagerAsync,
            Path path,
            string hint,
            Path basePath,
            IList<Path> excludedPaths ) : base(
            $"the requested path {path} is outside of the bounds of the hierarchy manager: {hint}" ) {
         HierarchyManagerAsync = hierarchyManagerAsync;
         Path = path;
         BasePath = basePath;
         ExcludedPaths = new ReadOnlyCollection<Path>( excludedPaths );
      }

      public IEnumerable<Path> ExcludedPaths { get; }

      public Path BasePath { get; }

      public Path Path { get; }

      public IHierarchyManagerAsync HierarchyManagerAsync { get; }

   }

}