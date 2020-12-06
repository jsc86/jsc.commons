// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.behaving;
using jsc.commons.hierarchy.acl.interfaces;
using jsc.commons.hierarchy.acl.serialization;
using jsc.commons.hierarchy.config;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.misc;

namespace jsc.commons.hierarchy.acl {

   public static class AclExtensions {

      public static IAccessControlList GetAccessControlList( this IResource resource, IHierarchyConfiguration config ) {
         resource.MustNotBeNull( nameof( resource ) );
         config.MustNotBeNull( nameof( config ) );

         if( !resource.Meta.TryGet( out AclMeta aclMeta ) )
            return new AccessControlList( );

         return AclSerializer.Deserialize( aclMeta.Acl, config );
      }

      public static void SetAccessControlList( this IResource resource, IAccessControlList acl ) {
         resource.MustNotBeNull( nameof( resource ) );
         acl.MustNotBeNull( nameof( acl ) );

         resource.Meta.Set( new AclMeta {Acl = AclSerializer.Serialize( acl )} );
      }

   }

}