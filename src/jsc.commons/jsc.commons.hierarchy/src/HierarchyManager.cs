using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using jsc.commons.hierarchy.config;
using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.hierarchy.users;

namespace jsc.commons.hierarchy {

   public class HierarchyManager : HierarchyManagerAsync, IHierarchyManager {

      public HierarchyManager(
            IHierarchy hierarchy,
            IHierarchyManagerConfiguration hierarchyManagerConfiguration = null ) : base(
            hierarchy,
            hierarchyManagerConfiguration ) {
         Hierarchy = hierarchy;
      }

      public IHierarchy Hierarchy { get; }

      public T Get<T>( User user, IPath path ) where T : IResource {
         Task<T> getTask = GetAsync<T>( user, path );
         getTask.Wait( Hierarchy.Timeout );
         return getTask.Result;
      }

      public bool TryGet<T>( User user, IPath path, out T resource ) where T : IResource {
         try {
            resource = Get<T>( user, path );
         } catch( Exception ) {
            resource = default;
            return false;
         }

         return true;
      }

      public void Set( User user, IResource resource ) {
         Task setTask = SetAsync( user, resource );
         setTask.Wait( Hierarchy.Timeout );
      }

      public void Delete( User user, IResource resource ) {
         Task deleteTask = DeleteAsync( user, resource );
         deleteTask.Wait( Hierarchy.Timeout );
      }

      public IEnumerable<string> GetChildrenResourceNames( User user, IPath path ) {
         Task<IEnumerable<string>> getTask = GetChildrenResourceNamesAsync( user, path );
         getTask.Wait( Hierarchy.Timeout );
         return getTask.Result;
      }

      public void Move( User user, IResource resource, IPath targetPath ) {
         Task moveTask = MoveAsync( user, resource, targetPath );
         moveTask.Wait( Hierarchy.Timeout );
      }

   }

}
