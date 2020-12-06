// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.hierarchy.meta;
using jsc.commons.hierarchy.meta.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;

namespace jsc.commons.hierarchy.resources {

   public abstract class ResourceBase : IResource {

      protected ResourceBase(
            IPath path,
            string name,
            IResourceClass resourceClass,
            IMeta meta = null ) : this( path, name, resourceClass, meta, false ) { }

      internal ResourceBase(
            IPath path,
            string name,
            IResourceClass resourceClass,
            IMeta meta = null,
            bool specialRootHandling = false ) {
         if( path == null )
            throw new ArgumentNullException( nameof( path ), $"{nameof( path )} must not be null" );
         if( !specialRootHandling
               &&name == null )
            throw new ArgumentNullException( nameof( name ), $"{nameof( name )} must not be null" );
         if( resourceClass == null )
            throw new ArgumentNullException( nameof( resourceClass ), $"{nameof( resourceClass )} must not be null" );

         Name = name;
         ResourceClass = resourceClass;
         Meta = meta??new Meta( );
         if( specialRootHandling )
            Path = path;
         else
            Path = path.Append( name );
      }

      public string Name { get; }
      public IMeta Meta { get; }
      public IResourceClass ResourceClass { get; }
      public IPath Path { get; }

      public override string ToString( ) {
         return Path.ToString( );
      }

   }

   public abstract class ResourceBase<T> : ResourceBase, IResource<T> where T : IResourceClass {

      protected ResourceBase(
            IPath path,
            string name,
            T resourceClass,
            IMeta meta = null ) : base( path, name, resourceClass, meta, false ) { }

      internal ResourceBase(
            IPath path,
            string name,
            IResourceClass resourceClass,
            IMeta meta = null,
            bool specialRootHandling = false ) : base( path, name, resourceClass, meta, specialRootHandling ) { }

      public T TypedResourceClass => (T)ResourceClass;

   }

}