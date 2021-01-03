// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;

using jsc.commons.async;
using jsc.commons.hierarchy.backend.interfaces;
using jsc.commons.hierarchy.config;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.misc;

namespace jsc.commons.hierarchy.backend {

   public class CacheBackend : IHierarchyBackend {

      private readonly IHierarchyBackend _backend;

      private readonly bool _backendSupportsMove;

      private readonly MemoryCache _cache;

      private readonly CacheItemPolicy _cacheItemPolicy;

      private readonly ExecutionTokenSpool _ets = new ExecutionTokenSpool( );

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

         try { // probe implementation of Move
            _backend.Move( null, null ).Wait( (int)TimeSpan.FromSeconds( 1 ).TotalMilliseconds );
         } catch( Exception exc ) {
            _backendSupportsMove = !( exc is NotImplementedException );
         }
      }

      public void Dispose( ) {
         if( _disposed )
            return;

         using( _ets.GetExecutionToken( ) ) {
            if( _disposed )
               return;

            _disposed = true;
            _backend.Dispose( );
            _cache.Dispose( );
         }

         _ets.Dispose( );
      }

      public async Task<IResource> Get( IPath path ) {
         path.MustNotBeNull( nameof( path ) );
         if( _disposed )
            throw new ObjectDisposedException( nameof( CacheBackend ) );

         string pathStr = path.ToString( );

         IResource resource = (IResource)_cache.Get( pathStr );
         if( resource == null ) {
            resource = await _backend.Get( path );
            using( await _ets.GetExecutionToken( ) ) {
               _cache.Set( pathStr, resource, _cacheItemPolicy );
            }
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

         using( await _ets.GetExecutionToken( ) ) {
            _cache.Set( resource.Path.ToString( ), resource, _cacheItemPolicy );
         }

         await _backend.Set( resource );
      }

      public async Task Delete( IResource resource ) {
         resource.MustNotBeNull( nameof( resource ) );
         if( _disposed )
            throw new ObjectDisposedException( nameof( CacheBackend ) );

         using( await _ets.GetExecutionToken( ) ) {
            _cache.Remove( resource.Path.ToString( ) );

            List<string> toBeRemoved = new List<string>( );
            foreach( KeyValuePair<string, object> kvp in _cache )
               if( IsSubPathOf( resource.Path, ( (IResource)kvp.Value ).Path ) )
                  toBeRemoved.Add( kvp.Key );

            foreach( string key in toBeRemoved )
               _cache.Remove( key );
         }

         await _backend.Delete( resource );
      }

      public async Task Move( IResource resource, IPath targetPath ) {
         if( _backendSupportsMove )
            throw new NotImplementedException( );

         IPath oldPath = resource.Path;
         await _backend.Move( resource, targetPath );

         List<string> toBeRemoved = new List<string>( );
         List<IResource> toBeAdded = new List<IResource>( );
         using( await _ets.GetExecutionToken( ) ) {
            foreach( KeyValuePair<string, object> kvp in _cache )
               if( IsSubPathOf( oldPath, Path.Parse( kvp.Key ) ) ) {
                  toBeRemoved.Add( kvp.Key );
                  IResource r = (IResource)kvp.Value;
                  toBeAdded.Add(
                        r.ResourceClass.CreateResource(
                              targetPath.Append( r.Path.RelativeTo( resource.Path ) ).Parent( ),
                              r.Name,
                              r.Meta ) );
               }

            foreach( string key in toBeRemoved )
               _cache.Remove( key );

            foreach( IResource r in toBeAdded )
               _cache[ r.Path.ToString( ) ] = r;
         }
      }

      private static bool IsSubPathOf( IPath sub, IPath path ) {
         IEnumerator<string> pathEnumerator = path.Elements.GetEnumerator( );
         try {
            foreach( string subElement in sub.Elements ) {
               if( !pathEnumerator.MoveNext( ) )
                  return false;

               if( subElement != pathEnumerator.Current )
                  return false;
            }

            return true;
         } finally {
            pathEnumerator.Dispose( );
         }
      }

   }

}
