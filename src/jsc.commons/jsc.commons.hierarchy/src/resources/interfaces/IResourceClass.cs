// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.hierarchy.acl.privileges.classes.interfaces;
using jsc.commons.hierarchy.meta.interfaces;
using jsc.commons.hierarchy.path.interfaces;

namespace jsc.commons.hierarchy.resources.interfaces {

   public interface IResourceClass {

      string Name { get; }

      ulong Id { get; }

      IEnumerable<IPrivilegeClass> ApplicablePrivileges { get; }

      IResource CreateResource( IPath path, string name, IMeta meta = null );

   }

}