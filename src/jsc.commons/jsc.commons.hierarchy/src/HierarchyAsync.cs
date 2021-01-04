// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using jsc.commons.hierarchy.backend.interfaces;
using jsc.commons.hierarchy.config;
using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.misc;

namespace jsc.commons.hierarchy {

   public class HierarchyAsync : IHierarchyAsync {

      private readonly IHierarchyBackend _backend;
      private volatile bool _disposed;
      private volatile bool _disposing;

      public HierarchyAsync( IHierarchyConfiguration configuration ) {
         configuration.MustNotBeNull( nameof( configuration ) );

         Configuration = configuration;

         _backend = configuration.BackendConfiguration.BackendFactory(
               configuration,
               configuration.BackendConfiguration );
      }

      public IHierarchyConfiguration Configuration { get; }

      public async Task<IResource> GetAsync( IPath path ) {
         CheckDisposed( );
         if( path == null )
            throw new ArgumentNullException( nameof( path ), $"{nameof( path )} must not be null" );

         return await _backend.Get( path );
      }


      public async Task<T> GetAsync<T>( IPath path ) where T : IResource {
         return (T)await GetAsync( path );
      }

      public async Task SetAsync( IResource resource ) {
         CheckDisposed( );
         if( resource == null )
            throw new ArgumentNullException( nameof( resource ), $"{nameof( resource )} must not be null" );

         await _backend.Set( resource );

         if( ResourceCreated != null )
            await ResourceCreated( this, new ResourceSetEventArgs( this, resource ) );
      }

      public async Task DeleteAsync( IResource resource ) {
         CheckDisposed( );
         resource.MustNotBeNull( nameof( resource ) );

         await _backend.Delete( resource );

         if( ResourceDeleted != null )
            await ResourceDeleted( this, new ResourceDeletedEventArgs( this, resource ) );
      }

      public async Task<IEnumerable<string>> GetChildrenResourceNamesAsync( IPath path ) {
         CheckDisposed( );
         if( path == null )
            throw new ArgumentNullException( nameof( path ), $"{nameof( path )} must not be null" );

         return await _backend.List( path );
      }

      public async Task MoveAsync( IResource resource, IPath targetPath ) {
         CheckDisposed( );
         resource.MustNotBeNull( nameof( resource ) );
         targetPath.MustNotBeNull( nameof( resource ) );

         try {
            await _backend.Move( resource, targetPath );
         } catch( NotImplementedException ) {
            if( !Configuration.AllowUseOfMoveFallback )
               throw new Exception(
                     $"The provided backend does not implement the {nameof( IHierarchyBackend.Move )} method. "
                     +$"The {nameof( HierarchyAsync )} implementation can leave the system in an undefined state "
                     +$"when encountering an error! If you want to use it, set {nameof( IHierarchyConfiguration )}."
                     +$"{nameof( IHierarchyConfiguration.AllowUseOfMoveFallback )} property to {true}." );

            await FallBackMoveAsync( resource, targetPath );
         }

         if( ResourceMoved != null )
            await ResourceMoved( this, new ResourceMovedEventArgs( this, resource, targetPath ) );
      }

      public event ResourceSetHandler ResourceCreated;
      public event ResourceDeletedHandler ResourceDeleted;
      public event ResourceMovedHandler ResourceMoved;

      public virtual void Dispose( ) {
         CheckDisposed( );
         _disposing = true;
         GC.SuppressFinalize( this );

         _backend.Dispose( );

         _disposed = true;
         _disposing = false;
      }

      private async Task FallBackMoveAsync( IResource resource, IPath targetPath ) {
         if( resource is IFileResource ) {
            IResource newResource = resource.ResourceClass.CreateResource( targetPath, resource.Name, resource.Meta );
            try {
               await _backend.Set( newResource );
            } catch( Exception exc ) {
               throw new Exception(
                     $"moving resource {resource.Path} failed "
                     +$"because the creation of the new resource {newResource.Path} failed",
                     exc );
            }

            try {
               await _backend.Delete( resource );
            } catch( Exception exc ) {
               try {
                  await _backend.Delete( newResource );
               } catch( Exception exc2 ) {
                  throw new Exception(
                        $"moving resource {resource.Path} failed because deleting it failed"+
                        $" and the newly created resource {newResource.Path} could not be removed again",
                        new AggregateException( exc, exc2 ) );
               }

               throw new Exception( $"moving resource {resource} failed because it could not be deleted" );
            }
         } else {
            await FallBackMoveAsyncRecursive( resource, targetPath );
         }
      }

      private async Task FallBackMoveAsyncRecursive( IResource resource, IPath targetPath ) {
         if( resource is IFileResource ) {
            await FallBackMoveAsync( resource, targetPath );
            return;
         }

         try {
            IResource newFolder = resource.ResourceClass.CreateResource( targetPath, resource.Name, resource.Meta );
            await _backend.Set( newFolder );
            foreach( string childResourceKey in await _backend.List( resource.Path ) ) {
               IResource childResource = await _backend.Get( resource.Path.Append( childResourceKey ) );
               await FallBackMoveAsyncRecursive( childResource, newFolder.Path );
            }

            await _backend.Delete( resource );
         } catch( Exception exc ) {
            throw new Exception(
                  $"catastrophic failure while moving resource {resource.Path}: "+
                  "the previous state was not restored",
                  exc );
         }
      }

      private void CheckDisposed( ) {
         if( _disposing||_disposed )
            throw new ObjectDisposedException( nameof( HierarchyAsync ) );
      }

      ~HierarchyAsync( ) {
         Dispose( );
      }

   }

}