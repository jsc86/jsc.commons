// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.misc;

namespace jsc.commons.hierarchy.path {

   public class Path : IPath {

      private static readonly Regex __pathSepRegex = new Regex( "/" );

      private readonly string[] _elements;
      private IPath _basePath;

      private string _stringRepresentation;

      private Path( string[] elements, bool absolute ) {
         _elements = elements;
         Absolute = absolute;
      }

      internal Path( IPath path ) {
         if( path == null )
            throw new ArgumentNullException( nameof( path ), $"{nameof( path )} must not be null" );

         _elements = path.Elements.ToArray( );
         Absolute = path.Absolute;
      }

      public Path( bool absolute ) {
         _elements = new string[0];
         Absolute = absolute;
      }

      public Path( string resourceName, bool absolute = true ) {
         if( resourceName == null )
            throw new ArgumentNullException( nameof( resourceName ), $"{nameof( resourceName )} must not be null" );
         if( resourceName.Length == 0 )
            throw new ArgumentException( $"{nameof( resourceName )} must not be empty", nameof( resourceName ) );

         _elements = new[] {resourceName};
         Absolute = absolute;
      }

      public Path( Path basePath, string resourceName ) {
         if( basePath == null )
            throw new ArgumentNullException( nameof( basePath ), $"{nameof( basePath )} must not be null" );
         if( resourceName == null )
            throw new ArgumentNullException( nameof( resourceName ), $"{nameof( resourceName )} must not be null" );
         if( resourceName.Length == 0 )
            throw new ArgumentException( $"{nameof( resourceName )} must not be empty", nameof( resourceName ) );

         _elements = new string[basePath._elements.Length+1];
         Array.Copy( basePath._elements, _elements, basePath._elements.Length );
         _elements[ _elements.Length-1 ] = resourceName;

         Absolute = basePath.Absolute;
         Elements = new EnumerableWrapper<string>( _elements );
      }

      public bool Absolute { get; }

      public IEnumerable<string> Elements { get; }
      public string Name => _elements.Last( );

      public IPath BasePath {
         get {
            if( _basePath == null ) {
               if( _elements.Length < 1 )
                  return null;

               string[] elements = new string[_elements.Length-1];
               Array.Copy( _elements, elements, elements.Length );
               _basePath = new Path( elements, Absolute );
            }

            return _basePath;
         }
      }

      public Path Append( string resourceName ) {
         return new Path( this, resourceName );
      }

      public Path Append( Path right ) {
         return Combine( this, right );
      }

      public override string ToString( ) {
         if( _stringRepresentation == null ) {
            StringBuilder sb = new StringBuilder( );
            if( Absolute )
               sb.Append( '/' );
            foreach( string element in _elements ) {
               sb.Append( __pathSepRegex.Replace( element, "\\/" ) );
               sb.Append( '/' );
            }

            if( sb.Length > 1 )
               sb.Remove( sb.Length-1, 1 );
            _stringRepresentation = sb.ToString( );
         }

         return _stringRepresentation;
      }

      public static Path Combine( Path left, Path right ) {
         if( left == null )
            throw new ArgumentNullException( nameof( left ), $"{nameof( left )} {nameof( Path )} must not be null" );
         if( right == null )
            throw new ArgumentNullException(
                  nameof( right ),
                  $"{nameof( right )} {nameof( Path )} path must not be null" );
         // if( right.Absolute )
         //    throw new ArgumentException( $"{nameof( right )} {nameof( Path )} must not be absolute" );

         string[] combinedElements = new string[left._elements.Length+right._elements.Length];
         Array.Copy( left._elements, 0, combinedElements, 0, left._elements.Length );
         Array.Copy( right._elements, 0, combinedElements, left._elements.Length, right._elements.Length );

         return new Path( combinedElements, left.Absolute );
      }

      public static Path Parse( string path ) {
         if( path == null )
            throw new ArgumentNullException( nameof( path ), $"{nameof( path )} must not be null" );

         string[] resourceNames = __pathSepRegex.Split( path );

         bool absolute = path.StartsWith( "/" );
         if( absolute ) {
            string[] temp = new string[resourceNames.Length-1];
            Array.Copy( resourceNames, 1, temp, 0, resourceNames.Length-1 );
            resourceNames = temp;
         }

         foreach( string resourceName in resourceNames )
            if( resourceName.Length == 0 )
               throw new ArgumentException( $"{nameof( resourceName )} must not be empty", nameof( resourceName ) );

         return new Path( resourceNames, absolute );
      }

   }

}