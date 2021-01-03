// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.misc;

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

      public static IPath Parent( this IPath path ) {
         path.MustNotBeNull( nameof( path ) );

         using IEnumerator<string> elementEnumerator = path.Elements.GetEnumerator( );
         string first;
         string second;
         if( elementEnumerator.MoveNext( ) )
            first = elementEnumerator.Current;
         else
            throw new Exception( "no elements in path" );

         if( elementEnumerator.MoveNext( ) )
            second = elementEnumerator.Current;
         else if( path.Absolute )
            return new Path( true );
         else
            throw new Exception( "not enough elements in path" );

         List<string> elements = new List<string> {first};

         while( elementEnumerator.MoveNext( ) ) {
            first = second;
            elements.Add( first );
            second = elementEnumerator.Current;
         }

         IPath parentPath = new Path( path.Absolute );
         foreach( string element in elements )
            parentPath = parentPath.Append( element );

         return parentPath;
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

   }

}
