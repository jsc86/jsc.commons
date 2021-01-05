// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.meta.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.resources.classes;
using jsc.commons.hierarchy.resources.interfaces;

namespace jsc.commons.hierarchy.resources {

   public abstract class FolderResourceBase : ResourceBase, IFolderResource {

      protected FolderResourceBase(
            Path path,
            string name,
            IResourceClass resourceClass,
            IMeta meta = null,
            bool specialRootHandling = false ) : base(
            path,
            name,
            resourceClass,
            meta,
            specialRootHandling ) { }

   }

   public abstract class FolderResourceBase<T> : FolderResourceBase, IFolderResource<T> where T : IResourceClass {

      protected FolderResourceBase(
            Path path,
            string name,
            T resourceClass,
            IMeta meta = null,
            bool specialRootHandling = false ) : base(
            path,
            name,
            resourceClass,
            meta,
            specialRootHandling ) { }

      public T TypedResourceClass => (T)ResourceClass;

   }

   public class Folder : FolderResourceBase<FolderResourceClass> {

      public Folder( Path path, string name, IMeta meta = null ) : base(
            path,
            name??string.Empty,
            FolderResourceClass.Instance,
            meta,
            Path.RootPath.Equals( path )&&string.IsNullOrEmpty( name ) ) { }

   }

}