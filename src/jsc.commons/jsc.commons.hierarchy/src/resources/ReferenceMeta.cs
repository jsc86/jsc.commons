// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Threading.Tasks;

using jsc.commons.behaving;
using jsc.commons.behaving.interfaces;
using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.meta;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.resources.interfaces;

namespace jsc.commons.hierarchy.resources {

   [MetaAutoModHandler( typeof( ReferenceMetaAutoModHandler ) )]
   public class ReferenceMeta : IBehavior {

      public string Path { get; set; }

      public override string ToString( ) {
         return $"meta: reference path {Path}";
      }

   }

   public class ReferenceMetaAutoModHandler : MetaAutoModHandlerBase {

      public override async Task OnMove( ResourceMovedEventArgs args, IHierarchyManagerAsync hierarchyManagerAsync ) {
         IResource oldResource = args.Resource;
         if( !oldResource.Meta.TryGet( out ReferenceMeta referenceMeta ) )
            return;

         Path path = Path.Parse( referenceMeta.Path );
         IResource otherResource = await hierarchyManagerAsync.HierarchyAsync.GetAsync<IResource>( path );

         if( !otherResource.Meta.TryGet( out BackReferenceMeta otherBackReferenceMeta ) ) {
            otherBackReferenceMeta = new BackReferenceMeta( );
            otherResource.Meta.Set( otherBackReferenceMeta );
         } else {
            otherBackReferenceMeta.BackReferences.Remove( oldResource.Path.ToString( ) );
         }

         otherBackReferenceMeta.BackReferences.Add( args.NewPath.Append( oldResource.Name ).ToString( ) );

         await hierarchyManagerAsync.HierarchyAsync.SetAsync( otherResource );
      }

      public override async Task OnDelete(
            ResourceDeletedEventArgs args,
            IHierarchyManagerAsync hierarchyManagerAsync ) {
         IResource resource = args.Resource;
         if( !resource.Meta.TryGet( out ReferenceMeta referenceMeta ) )
            return;

         Path path = Path.Parse( referenceMeta.Path );
         IResource otherResource = await hierarchyManagerAsync.HierarchyAsync.GetAsync<IResource>( path );

         if( !otherResource.Meta.TryGet( out BackReferenceMeta otherBackReferenceMeta ) )
            return;

         string resourcePath = resource.Path.ToString( );
         if( !otherBackReferenceMeta.BackReferences.Contains( resourcePath ) )
            return;

         otherBackReferenceMeta.BackReferences.Remove( resourcePath );
         await hierarchyManagerAsync.HierarchyAsync.SetAsync( otherResource );
      }

   }

}
