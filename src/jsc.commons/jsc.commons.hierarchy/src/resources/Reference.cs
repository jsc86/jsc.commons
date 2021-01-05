// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Threading.Tasks;

using jsc.commons.behaving;
using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.meta.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.resources.classes;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.misc;

namespace jsc.commons.hierarchy.resources {

   public class Reference : ResourceBase<ReferenceResourceClass> {

      public Reference( Path path, string name, IMeta meta = null ) :
            base( path, name, ReferenceResourceClass.Instance, meta ) {
         // nop
      }

      public async Task<IResource> LinkToAsync( Path path, IHierarchyAsync hierarchy ) {
         path.MustNotBeNull( nameof(path) );
         hierarchy.MustNotBeNull( nameof(hierarchy) );

         IResource resource = await hierarchy.GetAsync<IResource>( path );
         if( resource == null )
            throw new Exception( $"found no resource for path {path}" );

         LinkTo( resource );

         return resource;
      }

      public IResource LinkTo( Path path, IHierarchy hierarchy ) {
         path.MustNotBeNull( nameof(path) );
         hierarchy.MustNotBeNull( nameof(hierarchy) );

         IResource resource = hierarchy.Get<IResource>( path );
         if( resource == null )
            throw new Exception( $"found no resource for path {path}" );

         LinkTo( resource );

         return resource;
      }

      public void LinkTo( IResource resource ) {
         resource.MustNotBeNull( nameof(resource) );

         if( !Meta.TryGet( out ReferenceMeta referenceMeta ) ) {
            referenceMeta = new ReferenceMeta( );
            Meta.Set( referenceMeta );
         }

         referenceMeta.Path = resource.Path.ToString(  );

         resource.SetBackReference( this );
      }

      public Path GetLinkedResourcePath( ) {
         if( !Meta.TryGet( out ReferenceMeta referenceMeta ) )
            return null;

         if( referenceMeta.Path == null )
            return null;

         return path.Path.Parse( referenceMeta.Path );
      }

      public IResource GetLinkedResource( IHierarchy hierarchy ) {
         hierarchy.MustNotBeNull( nameof(hierarchy) );

         Path path = GetLinkedResourcePath( );

         return hierarchy.Get<IResource>( path );
      }
   }

}
