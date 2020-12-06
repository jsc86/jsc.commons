// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

namespace jsc.commons.hierarchy.acl.interfaces {

   public interface IAccessControlList {

      IEnumerable<IAccessControlRule> AccessControlRules { get; }

      void Add( IAccessControlRule acr );

      void Remove( IAccessControlRule acr );

   }

}