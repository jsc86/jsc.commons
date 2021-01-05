// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.meta.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.hierarchy.users;
using jsc.commons.misc;

namespace jsc.commons.hierarchy.groups {

   public class Group : FolderResourceBase<GroupResourceClass> {

      public Group( IPath path, string name, IMeta meta = null ) : base(
            path,
            name,
            GroupResourceClass.Instance,
            meta ) { }

      public IEnumerable<IPath> GetUserIDs( IHierarchy hierarchy ) {
         hierarchy.MustNotBeNull( nameof( hierarchy ) );

         foreach( string resourceName in hierarchy.GetChildrenResourceNames( Path ) )
            yield return Path.Append( resourceName );
      }

      public IEnumerable<User> GetUsers( IHierarchy hierarchy ) {
         hierarchy.MustNotBeNull( nameof( hierarchy ) );

         foreach( IPath userId in GetUserIDs( hierarchy ) )
            if( hierarchy.Get<IResource>( userId ) is User user )
               yield return user;
      }

   }

}