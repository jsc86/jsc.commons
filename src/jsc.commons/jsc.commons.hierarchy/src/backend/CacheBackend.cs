// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;

using jsc.commons.hierarchy.backend.interfaces;
using jsc.commons.hierarchy.config;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.misc;

namespace jsc.commons.hierarchy.backend {

   // TODO: add locking for cache operations

   public class CacheBackend : IHierarchyBackend {

      private readonly IHierarchyBackend _backend;

      private readonly MemoryCache _cache;

      private readonly CacheItemPolicy _cacheItemPolicy;

      private bool _disposed;

      public CacheBackend(
            IHierarchyConfiguration hierarchyConfiguration,
            ICacheBackendConfiguration configuration ) {
         hierarchyConfiguration.MustNotBeNull( nameof( hierarchyConfiguration ) );
         configuration.MustNotBeNull( nameof( configuration ) );
         configuration.NestedBackendConfiguration.MustNotBeNull( nameof( configuration.NestedBackendConfiguration ) );
         configuration.NestedBackendConfiguration.BackendFactory.MustNotBeNull(
               nameof( configuration.NestedBackendConfiguration.BackendFactory ) );
         configuration.CacheItemPolicy.MustNotBeNull( nameof( configuration.CacheItemPolicy ) );
         configuration.MemoryCacheConfiguration.MustNotBeNull( nameof( configuration.MemoryCacheConfiguration ) );

         _backend = configuration.NestedBackendConfiguration.BackendFactory(
               hierarchyConfiguration,
               configuration.NestedBackendConfiguration );
         _cacheItemPolicy = configuration.CacheItemPolicy;
         _cache = new MemoryCache( nameof( CacheBackend ), configuration.MemoryCacheConfiguration );
      }

      public void Dispose( ) {
         if( _disposed )
            return;

         _disposed = true;
         _backend.Dispose( );
         _cache.Dispose( );
      }

      public async Task<IResource> Get( IPath path ) {
         path.MustNotBeNull( nameof( path ) );
         if( _disposed )
            throw new ObjectDisposedException( nameof( CacheBackend ) );

         string pathStr = path.ToString( );

         IResource resource = (IResource)_cache.Get( pathStr );
         if( resource == null ) {
            resource = await _backend.Get( path );
            _cache.Set( pathStr, resource, _cacheItemPolicy );
         }

         return resource;
      }

      public async Task<IEnumerable<string>> List( IPath path ) {
         path.MustNotBeNull( nameof( path ) );
         if( _disposed )
            throw new ObjectDisposedException( nameof( CacheBackend ) );

         return await _backend.List( path );
      }

      public async Task Set( IResource resource ) {
         resource.MustNotBeNull( nameof( resource ) );
         if( _disposed )
            throw new ObjectDisposedException( nameof( CacheBackend ) );

         _cache.Set( resource.Path.ToString( ), resource, _cacheItemPolicy );
         await _backend.Set( resource );
      }

      public async Task Delete( IResource resource ) {
         resource.MustNotBeNull( nameof( resource ) );
         if( _disposed )
            throw new ObjectDisposedException( nameof( CacheBackend ) );

         _cache.Remove( resource.Path.ToString( ) );

         List<string> toBeRemoved = new List<string>( );
         foreach( KeyValuePair<string, object> kvp in _cache )
            if( IsSubPathOf( resource.Path, ( (IResource)kvp.Value ).Path ) )
               toBeRemoved.Add( kvp.Key );

         foreach( string key in toBeRemoved )
            _cache.Remove( key );

         await _backend.Delete( resource );
      }

      private static bool IsSubPathOf( IPath sub, IPath path ) {
         IEnumerator<string> pathEnumerator = path.Elements.GetEnumerator( );
         foreach( string subElement in sub.Elements ) {
            if( !pathEnumerator.MoveNext( ) )
               return false;

            if( subElement != pathEnumerator.Current )
               return false;
         }

         return true;
      }

   }

}
