// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Collections.ObjectModel;

using jsc.commons.hierarchy.acl.privileges.classes;
using jsc.commons.hierarchy.acl.privileges.classes.interfaces;
using jsc.commons.hierarchy.meta.interfaces;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;

namespace jsc.commons.hierarchy.resources.classes {

   public class ReferenceResourceClass : ResourceClass {

      private static readonly ReadOnlyCollection<IPrivilegeClass> __applicablePrivileges =
            new ReadOnlyCollection<IPrivilegeClass>(
                  new List<IPrivilegeClass> {
                        GrantPrivilegeClass.Instance,
                        ReadPrivilegeClass.Instance,
                        WritePrivilegeClass.Instance,
                        DeletePrivilegeClass.Instance
                  } );

      private ReferenceResourceClass( ) : base(
            "Reference",
            3,
            __applicablePrivileges ) { }

      public static ReferenceResourceClass Instance { get; } = new ReferenceResourceClass( );

      public override IResource CreateResource( IPath path, string name, IMeta meta = null ) {
         return new Reference( path, name, meta );
      }

   }

}