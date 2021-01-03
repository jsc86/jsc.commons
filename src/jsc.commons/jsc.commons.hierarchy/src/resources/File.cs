// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.meta.interfaces;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.classes;
using jsc.commons.hierarchy.resources.interfaces;

namespace jsc.commons.hierarchy.resources {

   public abstract class FileResourceBase<T> : ResourceBase<T>, IFileResource<T> where T : IResourceClass {

      protected FileResourceBase( IPath path, string name, T resourceClass, IMeta meta = null ) :
            base( path, name, resourceClass, meta ) { }

   }

   public class File : FileResourceBase<FileResourceClass> {

      public File( IPath path, string name, IMeta meta = null ) : base(
            path,
            name,
            FileResourceClass.Instance,
            meta ) { }

   }

}
