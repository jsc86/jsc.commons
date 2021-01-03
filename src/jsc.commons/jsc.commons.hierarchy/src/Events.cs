using System.Threading.Tasks;

using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.misc;

namespace jsc.commons.hierarchy {

   public class HierarchyEventArgs {

      protected HierarchyEventArgs( IHierarchyAsync hierarchyAsync ) {
         hierarchyAsync.MustNotBeNull( nameof( hierarchyAsync ) );

         HierarchyAsync = hierarchyAsync;
         Hierarchy = hierarchyAsync as IHierarchy;
      }

      public IHierarchy Hierarchy { get; }

      public IHierarchyAsync HierarchyAsync { get; }

   }

   public class ResourceEventArgs : HierarchyEventArgs {

      protected ResourceEventArgs( IHierarchyAsync hierarchyAsync, IResource resource ) : base( hierarchyAsync ) {
         resource.MustNotBeNull( nameof( resource ) );

         Resource = resource;
      }

      public IResource Resource { get; }

   }

   public class ResourceCreatedEventArgs : ResourceEventArgs {

      public ResourceCreatedEventArgs( IHierarchyAsync hierarchyAsync, IResource resource ) : base(
            hierarchyAsync,
            resource ) { }

   }

   public delegate Task ResourceCreatedHandler( object sender, ResourceCreatedEventArgs args );

   public class ResourceDeletedEventArgs : ResourceEventArgs {

      public ResourceDeletedEventArgs( IHierarchyAsync hierarchyAsync, IResource resource ) : base(
            hierarchyAsync,
            resource ) { }

   }

   public delegate Task ResourceDeletedHandler( object sender, ResourceDeletedEventArgs args );

   public class ResourceMovedEventArgs : ResourceEventArgs {

      public ResourceMovedEventArgs( IHierarchyAsync hierarchyAsync, IResource resource, IPath newPath ) : base(
            hierarchyAsync,
            resource ) {
         newPath.MustNotBeNull( nameof( newPath ) );

         NewPath = newPath;
      }

      public IPath NewPath { get; }

   }

   public delegate Task ResourceMovedHandler( object sender, ResourceMovedEventArgs args );

}
