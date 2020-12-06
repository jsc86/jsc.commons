// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.classes;

namespace jsc.commons.hierarchy.resources {

   public class RootFolder : FolderResourceBase<FolderResourceClass> {

      internal static readonly IPath RootPath = new Path( true );

      public RootFolder( ) : base(
            RootPath,
            string.Empty,
            FolderResourceClass.Instance,
            null,
            true ) { }

   }

}