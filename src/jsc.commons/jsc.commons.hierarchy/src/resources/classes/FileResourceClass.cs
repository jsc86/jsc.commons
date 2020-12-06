// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using jsc.commons.hierarchy.acl.privileges.classes;
using jsc.commons.hierarchy.acl.privileges.classes.interfaces;
using jsc.commons.hierarchy.meta.interfaces;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;

namespace jsc.commons.hierarchy.resources.classes {

   public class FileResourceClass : ResourceClass {

      private static readonly ReadOnlyCollection<IPrivilegeClass> __applicablePrivileges =
            new ReadOnlyCollection<IPrivilegeClass>(
                  new List<IPrivilegeClass> {
                        GrantPrivilegeClass.Instance,
                        ReadPrivilegeClass.Instance,
                        WritePrivilegeClass.Instance,
                        DeletePrivilegeClass.Instance
                  } );

      private FileResourceClass( ) : base(
            "File",
            new Guid( "00000000-0000-0000-0000-000000000002" ),
            __applicablePrivileges ) { }

      public static FileResourceClass Instance { get; } = new FileResourceClass( );

      public override IResource CreateResource( IPath path, string name, IMeta meta = null ) {
         return new File( path, name, meta );
      }

   }

}