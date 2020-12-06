// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using jsc.commons.hierarchy.config;
using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;

namespace jsc.commons.hierarchy {

   public class Hierarchy : HierarchyAsync, IHierarchy {

      private readonly TimeSpan _timeout;
      private bool _disposed = true;

      public Hierarchy( IHierarchyConfiguration conf = null ) : this( conf, TimeSpan.FromSeconds( 30 ) ) { }

      public Hierarchy( IHierarchyConfiguration conf, TimeSpan timeout ) : base( conf ) {
         _timeout = timeout;
      }

      ~Hierarchy( ) {
         Dispose( );
      }

      public override void Dispose( ) {
         if( _disposed )
            return;
         _disposed = true;
         GC.SuppressFinalize( this );
         base.Dispose( );
      }

      public IResource Get( IPath path ) {
         Task<IResource> getTask = GetAsync( path );
         getTask.Wait( _timeout );
         return getTask.Result;
      }

      public T Get<T, T2>( IPath path ) where T : IResource<T2> where T2 : IResourceClass {
         return (T)Get( path );
      }

      public bool TryGet( IPath path, out IResource resource ) {
         resource = null;
         try {
            resource = Get( path );
            return true;
         } catch( Exception ) {
            return false;
         }
      }

      public bool TryGet<T, T2>( IPath path, out T resource ) where T : IResource<T2> where T2 : IResourceClass {
         resource = default;
         try {
            resource = Get<T, T2>( path );
            return true;
         } catch( Exception ) {
            return false;
         }
      }

      public void Set( IResource resource ) {
         Task setTask = SetAsync( resource );
         setTask.Wait( _timeout );
      }

      public IEnumerable<string> GetChildrenResourceNames( IPath path ) {
         Task<IEnumerable<string>> getTask = GetChildrenResourceNamesAsync( path );
         getTask.Wait( _timeout );
         return getTask.Result;
      }

   }

}