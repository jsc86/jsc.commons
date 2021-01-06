// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Threading.Tasks;

using jsc.commons.hierarchy.interfaces;

namespace jsc.commons.hierarchy.meta.interfaces {

   public interface IMetaAutoModHandler {

      Task OnSet( ResourceSetEventArgs args, IHierarchyManagerAsync hierarchyManager );

      Task OnDelete( ResourceDeletedEventArgs args, IHierarchyManagerAsync hierarchyManagerAsync );

      Task OnMove( ResourceMovedEventArgs args, IHierarchyManagerAsync hierarchyManagerAsync );

   }

}