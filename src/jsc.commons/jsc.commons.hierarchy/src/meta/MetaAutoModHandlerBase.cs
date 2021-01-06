// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Threading.Tasks;

using jsc.commons.hierarchy.interfaces;
using jsc.commons.hierarchy.meta.interfaces;

namespace jsc.commons.hierarchy.meta {

   public class MetaAutoModHandlerBase : IMetaAutoModHandler {

      public virtual Task OnSet( ResourceSetEventArgs args, IHierarchyManagerAsync hierarchyManager ) {
         return Task.CompletedTask;
      }

      public virtual Task OnDelete( ResourceDeletedEventArgs args, IHierarchyManagerAsync hierarchyManagerAsync ) {
         return Task.CompletedTask;
      }

      public virtual Task OnMove( ResourceMovedEventArgs args, IHierarchyManagerAsync hierarchyManagerAsync ) {
         return Task.CompletedTask;
      }

   }

}