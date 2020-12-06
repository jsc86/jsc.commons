// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.meta.interfaces;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.resources;

namespace jsc.commons.hierarchy.users {

   public class User : FileResourceBase<UserResourceClass> {

      public User( IPath path, string name, IMeta meta = null ) : base(
            path,
            name,
            UserResourceClass.Instance,
            meta ) { }

   }

}