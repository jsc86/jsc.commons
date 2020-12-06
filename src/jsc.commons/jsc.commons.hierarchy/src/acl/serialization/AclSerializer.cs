// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.IO;
using System.Text;

using jsc.commons.hierarchy.acl.interfaces;
using jsc.commons.hierarchy.config;

namespace jsc.commons.hierarchy.acl.serialization {

   public static class AclSerializer {

      public static string Serialize( IAccessControlList acl ) {
         StringBuilder sb = new StringBuilder( );
         foreach( IAccessControlRule acr in acl.AccessControlRules ) {
            acr.ToString( sb );
            sb.Append( Environment.NewLine );
         }

         return sb.ToString( );
      }

      public static IAccessControlList Deserialize( string aclString, IHierarchyConfiguration config ) {
         using( TextReader tr = new StringReader( aclString ) ) {
            AccessControlList acl = new AccessControlList( );
            string line;
            while( ( line = tr.ReadLine( ) ) != null ) {
               if( string.IsNullOrWhiteSpace( line ) )
                  continue;

               IAccessControlRule acr = AccessControlRule.Parse( line, config );
               acl.Add( acr );
            }

            return acl;
         }
      }

   }

}