// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.hierarchy.acl.interfaces;
using jsc.commons.hierarchy.acl.privileges.interfaces;

namespace jsc.commons.hierarchy.acl {

   public class AcrEveryone : AccessControlRule {

      public AcrEveryone( EnAccessControlAction action, IEnumerable<IPrivilege> privileges ) : base(
            action,
            null,
            privileges ) { }

      protected override string ToPrefix => "EVERYONE";

   }

}