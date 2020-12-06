// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.meta.interfaces;
using jsc.commons.hierarchy.path.interfaces;

namespace jsc.commons.hierarchy.resources.interfaces {

   public interface IResource {

      string Name { get; }

      IMeta Meta { get; }

      IResourceClass ResourceClass { get; }

      IPath Path { get; }

   }

   public interface IResource<T> : IResource where T : IResourceClass {

      T TypedResourceClass { get; }

   }

}