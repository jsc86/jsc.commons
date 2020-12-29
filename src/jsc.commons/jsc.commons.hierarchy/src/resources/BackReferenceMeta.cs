// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Text;

using jsc.commons.behaving;
using jsc.commons.behaving.interfaces;
using jsc.commons.hierarchy.resources.interfaces;

namespace jsc.commons.hierarchy.resources {

   public class BackReferenceMeta : IBehavior {

      public List<string> BackReferences { get; set; } = new List<string>( );

      public override string ToString( ) {
         if( BackReferences == null
               ||BackReferences.Count == 0 )
            return "meta: back references {}";
         StringBuilder sb = new StringBuilder( );
         sb.Append( "meta: back references" );
         sb.Append( Environment.NewLine );
         sb.Append( "{" );
         sb.Append( Environment.NewLine );
         foreach( string backReference in BackReferences ) {
            sb.Append( "  " );
            sb.Append( backReference );
            sb.Append( "," );
            sb.Append( Environment.NewLine );
         }

         int l = 1+Environment.NewLine.Length;
         sb.Remove( sb.Length-l, l );
         sb.Append( Environment.NewLine );
         sb.Append( "}" );

         return sb.ToString( );
      }

   }

   public static class BackReferenceExtensions {

      public static void SetBackReference( this IResource referenceResource, IResource backReferenceResource ) {
         if( !referenceResource.Meta.TryGet( out BackReferenceMeta backReferenceMeta ) ) {
            backReferenceMeta = new BackReferenceMeta( );
            referenceResource.Meta.Set( backReferenceMeta );
         }

         string backReferencePath = backReferenceResource.Path.ToString( );
         if( !backReferenceMeta.BackReferences.Contains( backReferencePath ) )
            backReferenceMeta.BackReferences.Add( backReferencePath );
      }

   }

}
