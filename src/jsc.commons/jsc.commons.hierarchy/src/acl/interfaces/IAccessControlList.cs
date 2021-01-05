// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.hierarchy.acl.privileges.interfaces;
using jsc.commons.hierarchy.path;

namespace jsc.commons.hierarchy.acl.interfaces {

   public interface IAccessControlList {

      IEnumerable<IAccessControlRule> AccessControlRules { get; }

      void Add( IAccessControlRule acr );

      void Remove( IAccessControlRule acr );

      bool? HasPrivilege( Path userPath, IList<Path> groupPaths, IPrivilege privilege );

   }

}