// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.resources;

namespace jsc.commons.hierarchy.users {

   public class Everyone : ResourceBase<UserResourceClass> {

      public Everyone( ) : base( path.Path.RootPath, "everyone", UserResourceClass.Instance, null, false ) { }

   }

}