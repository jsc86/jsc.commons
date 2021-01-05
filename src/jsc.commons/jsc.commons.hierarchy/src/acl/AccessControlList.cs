// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;

using jsc.commons.hierarchy.acl.interfaces;
using jsc.commons.hierarchy.acl.privileges.interfaces;
using jsc.commons.hierarchy.path.interfaces;
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

      public bool? HasPrivilege( IPath userPath, IList<IPath> groupPaths, IPrivilege privilege ) {
         foreach( IAccessControlRule acr in _acrs.Reverse( ) )
            switch( acr ) {
               case AcrInherit acrInherit:
                  return null;
               case AcrEveryone acrEveryone:
                  if( acr.Privileges.Contains( privilege ) )
                     return acr.Action == EnAccessControlAction.Allow;
                  break;
               case AcrGroup acrGroup:
                  if( groupPaths.Contains( acrGroup.ToPath )
                        &&acr.Privileges.Contains( privilege ) )
                     return acr.Action == EnAccessControlAction.Allow;
                  break;
               case AcrUser acrUser:
                  if( acr.ToPath == userPath
                        &&acr.Privileges.Contains( privilege ) )
                     return acr.Action == EnAccessControlAction.Allow;
                  break;
               default:
                  throw new Exception( $"unknown ACR type {acr.GetType( )}" );
            }

         return null;
      }

   }

}
