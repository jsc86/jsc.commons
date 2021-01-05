// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.behaving;
using jsc.commons.hierarchy.groups;
using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.meta.interfaces;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources;
using jsc.commons.misc;

namespace jsc.commons.hierarchy.users {

   public class User : FileResourceBase<UserResourceClass> {

      public User( IPath path, string name, IMeta meta = null ) : base(
            path,
            name,
            UserResourceClass.Instance,
            meta ) { }

      public IEnumerable<IPath> GetGroupIDs( ) {
         if( !this.TryGet( out BackReferenceMeta backReferences )
               ||backReferences.BackReferences.Count == 0 )
            yield break;

         foreach( string pathString in backReferences.BackReferences )
            yield return path.Path.Parse( pathString );
      }

      public IEnumerable<Group> GetGroups( IHierarchy hierarchy ) {
         hierarchy.MustNotBeNull( nameof( hierarchy ) );

         foreach( IPath groupId in GetGroupIDs( ) )
            if( hierarchy.Get( groupId ) is Group group )
               yield return group;
      }

      public async IAsyncEnumerable<Group> GetGroupsAsync( IHierarchyAsync hierarchy ) {
         hierarchy.MustNotBeNull( nameof( hierarchy ) );

         foreach( IPath groupId in GetGroupIDs( ) )
            if( await hierarchy.GetAsync( groupId ) is Group group )
               yield return group;
      }

   }

}
