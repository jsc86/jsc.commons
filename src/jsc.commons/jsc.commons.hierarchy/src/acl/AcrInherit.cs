// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Text;

using jsc.commons.hierarchy.acl.interfaces;

namespace jsc.commons.hierarchy.acl {

   public class AcrInherit : AccessControlRule {

      public AcrInherit( ) : base(
            EnAccessControlAction.Inherit,
            null,
            null ) { }

      public override string ToString( ) {
         return Action.ToString( ).ToUpper( );
      }

      public override void ToString( StringBuilder sb ) {
         sb.Append( ToString( ) );
      }

   }

}