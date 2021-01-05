// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using jsc.commons.hierarchy.config;
using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.resources.interfaces;

namespace jsc.commons.hierarchy {

   public class Hierarchy : HierarchyAsync, IHierarchy {

      private bool _disposed = true;

      public Hierarchy( IHierarchyConfiguration configuration = null ) : this(
            configuration,
            TimeSpan.FromSeconds( 30 ) ) { }

      public Hierarchy( IHierarchyConfiguration configuration, TimeSpan timeout ) : base( configuration ) {
         Timeout = timeout;
      }

      public TimeSpan Timeout { get; }

      public override void Dispose( ) {
         if( _disposed )
            return;
         _disposed = true;
         GC.SuppressFinalize( this );
         base.Dispose( );
      }

      public T Get<T>( Path path ) where T : IResource {
         Task<T> getTask = GetAsync<T>( path );
         getTask.Wait( Timeout );
         return getTask.Result;
      }

      public bool TryGet<T>( Path path, out T resource ) where T : IResource {
         resource = default;
         try {
            resource = Get<T>( path );
            return true;
         } catch( Exception ) {
            return false;
         }
      }

      public void Set( IResource resource ) {
         Task setTask = SetAsync( resource );
         setTask.Wait( Timeout );
      }

      public void Delete( IResource resource ) {
         Task deleteTask = DeleteAsync( resource );
         deleteTask.Wait( Timeout );
      }

      public IEnumerable<string> GetChildrenResourceNames( Path path ) {
         Task<IEnumerable<string>> getTask = GetChildrenResourceNamesAsync( path );
         getTask.Wait( Timeout );
         return getTask.Result;
      }

      public void Move( IResource resource, Path targetPath ) {
         Task moveTask = MoveAsync( resource, targetPath );
         moveTask.Wait( Timeout );
      }

      ~Hierarchy( ) {
         Dispose( );
      }

   }

}
