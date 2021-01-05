// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Collections.ObjectModel;

using jsc.commons.hierarchy.acl.privileges.classes;
using jsc.commons.hierarchy.acl.privileges.classes.interfaces;
using jsc.commons.hierarchy.meta.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.resources.interfaces;

namespace jsc.commons.hierarchy.resources.classes {

   public class FolderResourceClass : ResourceClass {

      private static readonly ReadOnlyCollection<IPrivilegeClass> __applicablePrivileges =
            new ReadOnlyCollection<IPrivilegeClass>(
                  new List<IPrivilegeClass> {
                        GrantPrivilegeClass.Instance,
                        ReadPrivilegeClass.Instance,
                        WritePrivilegeClass.Instance,
                        CreatePrivilegeClass.Instance,
                        DeletePrivilegeClass.Instance
                  } );

      private FolderResourceClass( ) : base(
            "Folder",
            1,
            __applicablePrivileges ) { }

      public static FolderResourceClass Instance { get; } = new FolderResourceClass( );

      public override IResource CreateResource( Path path, string name, IMeta meta = null ) {
         return new Folder( path, name, meta );
      }

   }

}