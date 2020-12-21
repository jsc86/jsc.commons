// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using jsc.commons.hierarchy.acl.privileges.classes.interfaces;
using jsc.commons.hierarchy.meta.interfaces;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;

namespace jsc.commons.hierarchy.resources.classes {

   public abstract class ResourceClass : IResourceClass {

      public ResourceClass( string name, ulong id, IEnumerable<IPrivilegeClass> applicablePrivileges ) {
         if( name == null )
            throw new ArgumentNullException( nameof( name ), $"{nameof( name )} must not be null" );

         Name = name;
         Id = id;
         ApplicablePrivileges = applicablePrivileges == null
               ? Enumerable.Empty<IPrivilegeClass>( )
               : applicablePrivileges as ReadOnlyCollection<IPrivilegeClass>
               ??new ReadOnlyCollection<IPrivilegeClass>(
                     applicablePrivileges as List<IPrivilegeClass>??applicablePrivileges.ToList( ) );
      }

      public string Name { get; }
      public ulong Id { get; }
      public IEnumerable<IPrivilegeClass> ApplicablePrivileges { get; }
      public abstract IResource CreateResource( IPath path, string name, IMeta meta = null );

   }

}
