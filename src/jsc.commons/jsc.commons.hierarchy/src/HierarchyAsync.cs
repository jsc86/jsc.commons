// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

using jsc.commons.async;
using jsc.commons.hierarchy.backend.interfaces;
using jsc.commons.hierarchy.config;
using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.misc;

namespace jsc.commons.hierarchy {

   public class HierarchyAsync : IHierarchyAsync {

      private readonly IHierarchyBackend _backend;
      private readonly Timer _cleanupTimer;
      private readonly ExecutionTokenSpool _ets;
      private readonly Dictionary<IPath, WeakReference<IResource>> _resources;
      private volatile bool _disposed;
      private volatile bool _disposing;

      public HierarchyAsync( IHierarchyConfiguration configuration ) {
         configuration.MustNotBeNull( nameof( configuration ) );

         _backend = configuration.BackendConfiguration.BackendFactory(
               configuration,
               configuration.BackendConfiguration );
         _resources = new Dictionary<IPath, WeakReference<IResource>>( );
         _ets = new ExecutionTokenSpool( );
         _cleanupTimer = new Timer( TimeSpan.FromSeconds( 10 ).TotalMilliseconds );
         _cleanupTimer.Elapsed += OnCleanupTimer;
      }

      public IHierarchyConfiguration Configuration { get; }

      public async Task<IResource> GetAsync( IPath path ) {
         CheckDisposed( );
         if( path == null )
            throw new ArgumentNullException( nameof( path ), $"{nameof( path )} must not be null" );

         IResource resource = null;
         bool loaded;
         WeakReference<IResource> wref;

         using( await _ets.GetExecutionToken( ) ) {
            loaded = _resources.TryGetValue( path, out wref );
         }

         if( !loaded ) {
            wref = new WeakReference<IResource>( resource = await _backend.Get( path ) );
            using( await _ets.GetExecutionToken( ) ) {
               _resources[ resource.Path ] = wref;
            }
         }

         if( resource == null
               &&!wref.TryGetTarget( out resource ) ) {
            resource = await _backend.Get( path );
            wref.SetTarget( resource );
         }

         return resource;
      }


      public async Task<T> GetAsync<T, T2>( IPath path ) where T : IResource<T2> where T2 : IResourceClass {
         return (T)await GetAsync( path );
      }

      public async Task SetAsync( IResource resource ) {
         CheckDisposed( );
         if( resource == null )
            throw new ArgumentNullException( nameof( resource ), $"{nameof( resource )} must not be null" );

         await _backend.Set( resource );

         using( await _ets.GetExecutionToken( ) ) {
            _resources[ resource.Path ] = new WeakReference<IResource>( resource );
         }
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

         Task<ExecutionTokenSpool.ExecutionToken> execTokenTask = _ets.GetExecutionToken( );
         execTokenTask.Wait( TimeSpan.FromSeconds( 15 ) );
         using( execTokenTask.Result ) {
            _cleanupTimer.Dispose( );
         }

         _ets.Dispose( );
         _backend.Dispose( );

         _disposed = true;
         _disposing = false;
      }

      private async void OnCleanupTimer( object sender, ElapsedEventArgs e ) {
         using( await _ets.GetExecutionToken( ) ) {
            int count = _resources.Count;
            if( count < 1000 )
               return;

            List<KeyValuePair<IPath, WeakReference<IResource>>> kvpList = _resources.ToList( );
            List<IPath> toBeRemoved = new List<IPath>( );

            Random rand = new Random( );
            for( int i = 0,
                  l = (int)Math.Min( count*0.2, 1000 );
                  i < l;
                  i++ ) {
               KeyValuePair<IPath, WeakReference<IResource>> kvp = kvpList[ rand.Next( 0, count ) ];
               if( !kvp.Value.TryGetTarget( out IResource resource ) )
                  toBeRemoved.Add( kvp.Key );
            }

            foreach( IPath key in toBeRemoved )
               _resources.Remove( key );
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