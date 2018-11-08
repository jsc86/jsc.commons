// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

namespace jsc.commons.naming.interfaces {

   /// <summary>
   ///    A Naming Style defines rules for parsing and formatting
   ///    Unified Names.
   /// </summary>
   public interface INamingStyle {

      string Name { get; }

      /// <summary>
      ///    Format the given Unified Name according to this Naming Style.
      /// </summary>
      /// <param name="unifiedName"></param>
      /// <returns></returns>
      string ToString( UnifiedName unifiedName );

      /// <summary>
      ///    Parse the given string to a Unified Name according to this Naming Style.
      /// </summary>
      /// <param name="name"></param>
      /// <returns></returns>
      UnifiedName FromString( string name );

   }

}