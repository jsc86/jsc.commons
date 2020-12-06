// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

namespace jsc.commons.hierarchy.resources.interfaces {

   public interface IFileResource : IResource { }

   public interface IFileResource<T> : IFileResource, IResource<T> where T : IResourceClass { }

}