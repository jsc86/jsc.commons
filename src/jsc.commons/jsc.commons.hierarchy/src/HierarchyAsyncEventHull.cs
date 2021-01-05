// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using jsc.commons.hierarchy.config;
using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.misc;

namespace jsc.commons.hierarchy {

   public class HierarchyAsyncEventHull : IHierarchyAsync {

      private readonly IHierarchyAsync _hierarchy;

      public HierarchyAsyncEventHull( IHierarchyAsync hierarchy ) {
         hierarchy.MustNotBeNull( nameof( hierarchy ) );

         _hierarchy = hierarchy;

         _hierarchy.ResourceCreated += OnResourceCreated;
         _hierarchy.ResourceDeleted += OnResourceDeleted;
         _hierarchy.ResourceMoved += OnResourceMoved;
      }

      public void Dispose( ) {
         _hierarchy.Dispose( );
      }

      public IHierarchyConfiguration Configuration => _hierarchy.Configuration;


      public Task<T> GetAsync<T>( Path path ) where T : IResource {
         return _hierarchy.GetAsync<T>( path );
      }

      public Task SetAsync( IResource resource ) {
         return _hierarchy.SetAsync( resource );
      }

      public async Task DeleteAsync( IResource resource ) {
         IEnumerable<IResource> deletedResources = null;
         if( ResourceDeleted != null
               &&resource is IFolderResource folder )
            deletedResources = await TraverseResources( folder );

         await _hierarchy.DeleteAsync( resource );


         if( deletedResources != null )
            foreach( IResource deletedResource in deletedResources )
               await ResourceDeleted( this, new ResourceDeletedEventArgs( this, deletedResource ) );
      }

      public Task<IEnumerable<string>> GetChildrenResourceNamesAsync( Path path ) {
         return _hierarchy.GetChildrenResourceNamesAsync( path );
      }

      public async Task MoveAsync( IResource resource, Path targetPath ) {
         Path oldPath = resource.Path;
         IEnumerable<IResource> movedResources = null;
         if( ResourceMoved != null
               &&resource is IFolderResource folder )
            movedResources = await TraverseResources( folder );

         await _hierarchy.MoveAsync( resource, targetPath );

         if( movedResources != null )
            foreach( ResourceMovedEventArgs args in movedResources.Select(
                  r => new ResourceMovedEventArgs(
                        this,
                        r,
                        targetPath.Append( r.Path.RelativeTo( oldPath ) )
                  ) ) )
               await ResourceMoved( this, args );
      }

      public event ResourceSetHandler ResourceCreated;
      public event ResourceDeletedHandler ResourceDeleted;
      public event ResourceMovedHandler ResourceMoved;

      private Task OnResourceMoved( object sender, ResourceMovedEventArgs args ) {
         return ResourceMoved?.Invoke( this, new ResourceMovedEventArgs( this, args.Resource, args.NewPath ) );
      }

      private Task OnResourceDeleted( object sender, ResourceDeletedEventArgs args ) {
         return ResourceDeleted?.Invoke( this, new ResourceDeletedEventArgs( this, args.Resource ) );
      }

      private Task OnResourceCreated( object sender, ResourceSetEventArgs args ) {
         return ResourceCreated?.Invoke( sender, new ResourceSetEventArgs( this, args.Resource ) );
      }

      private async Task<IEnumerable<IResource>> TraverseResources( IFolderResource resource ) {
         IEnumerable<string> childResourceNames = await _hierarchy.GetChildrenResourceNamesAsync( resource.Path );
         IList<IResource> childResources = new List<IResource>( );
         foreach( string resourceName in childResourceNames )
            childResources.Add( await _hierarchy.GetAsync<IResource>( resource.Path.Append( resourceName ) ) );

         IEnumerable<IResource> allResources = new EnumerableWrapper<IResource>( childResources );
         foreach( IResource childResource in childResources )
            if( childResource is IFolderResource folder )
               allResources = allResources.Union( await TraverseResources( folder ) );

         return allResources;
      }

   }

}
