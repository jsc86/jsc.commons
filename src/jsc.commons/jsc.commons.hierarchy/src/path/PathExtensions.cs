// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.misc;

namespace jsc.commons.hierarchy.path {

   public static class PathExtensions {

      public static IPath Append( this IPath left, IPath right ) {
         Path pLeft = left as Path??new Path( left );
         Path pRight = right as Path??new Path( right );
         return pLeft.Append( pRight );
      }

      public static IPath Append( this IPath left, string resourceName ) {
         Path pLeft = left as Path??new Path( left );
         return pLeft.Append( resourceName );
      }

      public static IPath RelativeTo( this IPath left, IPath right ) {
         left.MustNotBeNull( nameof( left ) );
         right.MustNotBeNull( nameof( right ) );

         if( left.Absolute != right.Absolute )
            throw new ArgumentException(
                  $"both paths ({nameof( left )} and {nameof( right )}) must be absolute or relative" );

         string leftString = left.ToString( );
         string rightString = right.ToString( );

         if( !leftString.StartsWith( rightString ) )
            throw new Exception( $"{nameof( left )} is not a sub path of {nameof( right )}" );

         return Path.Parse( leftString.Substring( rightString.Length, leftString.Length-rightString.Length ) );
      }

      public static bool IsContainedIn( this IPath left, IPath right ) {
         left.MustNotBeNull( nameof( left ) );
         right.MustNotBeNull( nameof( right ) );

         if( left.Absolute != right.Absolute )
            throw new ArgumentException(
                  $"both paths ({nameof( left )} and {nameof( right )}) must be absolute or relative" );

         string leftString = left.ToString( );
         string rightString = right.ToString( );

         return leftString.StartsWith( rightString )&&leftString != rightString;
      }

   }

}
