// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.hierarchy.acl.interfaces;
using jsc.commons.misc;

namespace jsc.commons.hierarchy.acl {

   public class AccessControlList : IAccessControlList {

      private readonly IList<IAccessControlRule> _acrs;

      public AccessControlList( ) {
         _acrs = new List<IAccessControlRule>( );
      }

      public IEnumerable<IAccessControlRule> AccessControlRules => new EnumerableWrapper<IAccessControlRule>( _acrs );

      public void Add( IAccessControlRule acr ) {
         acr.MustNotBeNull( nameof( acr ) );

         _acrs.Add( acr );
      }

      public void Remove( IAccessControlRule acr ) {
         acr.MustNotBeNull( nameof( acr ) );

         _acrs.Remove( acr );
      }

   }

}