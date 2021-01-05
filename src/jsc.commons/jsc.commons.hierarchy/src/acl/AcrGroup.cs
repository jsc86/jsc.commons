// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.hierarchy.acl.interfaces;
using jsc.commons.hierarchy.acl.privileges.interfaces;
using jsc.commons.hierarchy.path;

namespace jsc.commons.hierarchy.acl {

   public class AcrGroup : AccessControlRule {

      public AcrGroup(
            EnAccessControlAction action,
            Path toPath,
            IEnumerable<IPrivilege> privileges ) : base( action, toPath, privileges ) { }

      protected override string ToPrefix => "GROUP ";

   }

}