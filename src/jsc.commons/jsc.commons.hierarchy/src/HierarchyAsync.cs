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


      public async Task<T> GetAsync<T, T2>( IPath path ) where T : IResource<T2> where T2 : IResourceClass {
         return (T)await GetAsync( path );
      }

      public async Task SetAsync( IResource resource ) {
         CheckDisposed( );
         if( resource == null )
            throw new ArgumentNullException( nameof( resource ), $"{nameof( resource )} must not be null" );

         await _backend.Set( resource );
      }

      public async Task DeleteAsync( IResource resource ) {
         CheckDisposed( );
         resource.MustNotBeNull( nameof( resource ) );

         await _backend.Delete( resource );
      }

      public async Task<IEnumerable<string>> GetChildrenResourceNamesAsync( IPath path ) {
         CheckDisposed( );
         if( path == null )
            throw new ArgumentNullException( nameof( path ), $"{nameof( path )} must not be null" );

         return await _backend.List( path );
      }

      public virtual void Dispose( ) {
         CheckDisposed( );
         _disposing = true;
         GC.SuppressFinalize( this );

         _backend.Dispose( );

         _disposed = true;
         _disposing = false;
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
