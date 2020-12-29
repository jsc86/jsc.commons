// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Text;

using jsc.commons.behaving.interfaces;

namespace jsc.commons.hierarchy.acl {

   public class AclMeta : IBehavior {

      public string Acl { get; set; }

      public override string ToString( ) {
         StringBuilder sb = new StringBuilder( );
         sb.Append( "meta: ACL:" );
         sb.Append( Environment.NewLine );
         sb.Append( Acl );
         return sb.ToString( );
      }

   }

}
