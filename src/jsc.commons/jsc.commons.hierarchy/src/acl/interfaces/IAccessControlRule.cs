// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Text;

using jsc.commons.hierarchy.acl.privileges.interfaces;
using jsc.commons.hierarchy.path.interfaces;

namespace jsc.commons.hierarchy.acl.interfaces {

   public interface IAccessControlRule {

      EnAccessControlAction Action { get; }

      IPath ToPath { get; }

      IEnumerable<IPrivilege> Privileges { get; }

      string ToString( );

      void ToString( StringBuilder sb );

   }

}