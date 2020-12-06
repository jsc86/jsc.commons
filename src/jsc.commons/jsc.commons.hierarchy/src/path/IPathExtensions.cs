// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.path.interfaces;

namespace jsc.commons.hierarchy.path {

   public static class IPathExtensions {

      public static IPath Append( this IPath left, IPath right ) {
         Path pLeft = left as Path??new Path( left );
         Path pRight = right as Path??new Path( right );
         return pLeft.Append( pRight );
      }

      public static IPath Append( this IPath left, string resourceName ) {
         Path pLeft = left as Path??new Path( left );
         return pLeft.Append( resourceName );
      }

   }

}