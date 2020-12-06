// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace jsc.commons.misc {

   public static class Enumerable {

      public static IEnumerable<T> SelectManyRecursive<T>(
            this T source,
            Func<T, IEnumerable<T>> selector ) {
         if( source == null )
            throw new ArgumentNullException( nameof( source ), $"{nameof( source )} must not be null" );
         if( selector == null )
            throw new ArgumentNullException( nameof( selector ), $"{nameof( selector )} must not be null" );

         return selector( source ).SelectManyRecursiveInclusive( selector );
      }

      public static IEnumerable<T> SelectManyRecursiveInclusive<T>(
            this T source,
            Func<T, IEnumerable<T>> selector ) {
         if( source == null )
            throw new ArgumentNullException( nameof( source ), $"{nameof( source )} must not be null" );
         if( selector == null )
            throw new ArgumentNullException( nameof( selector ), $"{nameof( selector )} must not be null" );

         return new[] {source}.Union( selector( source ).SelectManyRecursiveInclusive( selector ) );
      }


      public static IEnumerable<T> SelectManyRecursive<T>(
            this IEnumerable<T> source,
            Func<T, IEnumerable<T>> selector ) {
         if( source == null )
            throw new ArgumentNullException( nameof( source ), $"{nameof( source )} must not be null" );
         if( selector == null )
            throw new ArgumentNullException( nameof( selector ), $"{nameof( selector )} must not be null" );

         source = source.PrepareForMultipleEnumeration( );

         IEnumerable<T> ret = System.Linq.Enumerable.Empty<T>( );

         return source.Aggregate(
               ret,
               ( current, x ) => current.Union( selector( x ).SelectManyRecursive( selector ) ) );
      }

      public static IEnumerable<T> SelectManyRecursiveInclusive<T>(
            this IEnumerable<T> source,
            Func<T, IEnumerable<T>> selector ) {
         if( source == null )
            throw new ArgumentNullException( nameof( source ), $"{nameof( source )} must not be null" );
         if( selector == null )
            throw new ArgumentNullException( nameof( selector ), $"{nameof( selector )} must not be null" );

         source = source.PrepareForMultipleEnumeration( );

         // ReSharper disable PossibleMultipleEnumeration
         return source.Union( source.SelectManyRecursive( selector ) );
         // ReSharper restore PossibleMultipleEnumeration
      }

      public static IEnumerable<T> PrepareForMultipleEnumeration<T>( this IEnumerable<T> source ) {
         if( source is ICollection
               ||source is ICollection<T> )
            return source;
         return source.ToList( );
      }

   }

}