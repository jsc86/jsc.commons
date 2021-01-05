// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using jsc.commons.behaving;
using jsc.commons.hierarchy.groups;
using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.meta.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.resources;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.misc;

using Enumerable = System.Linq.Enumerable;

namespace jsc.commons.hierarchy.users {

   public class User : FileResourceBase<UserResourceClass> {

      public User( Path path, string name, IMeta meta = null ) : base(
            path,
            name,
            UserResourceClass.Instance,
            meta ) { }

      public IEnumerable<Path> GetGroupIDs( ) {
         if( !Meta.TryGet( out BackReferenceMeta backReferences )
               ||backReferences.BackReferences.Count == 0 )
            return Enumerable.Empty<Path>( );

         return backReferences.BackReferences.Select( Path.Parse );
      }

      public IEnumerable<Group> GetGroups( IHierarchy hierarchy ) {
         hierarchy.MustNotBeNull( nameof( hierarchy ) );

         foreach( Path groupId in GetGroupIDs( ) )
            if( hierarchy.Get<IResource>( groupId ) is Group group )
               yield return group;
      }

      public async Task<IEnumerable<Group>> GetGroupsAsync( IHierarchyAsync hierarchy ) {
         hierarchy.MustNotBeNull( nameof( hierarchy ) );

         List<Group> groups = new List<Group>( );

         foreach( Path groupId in GetGroupIDs( ) )
            if( await hierarchy.GetAsync<IResource>( groupId ) is Group group )
               groups.Add( group );

         return groups;
      }

   }

}