// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

namespace jsc.commons.config.interfaces {

   /// <summary>
   ///    A Defaults Provider is meant to provide default values for
   ///    configuration properties which can not be defined by constants
   ///    (i.e. non-value types).
   /// </summary>
   public interface IDefaultsProvider {

      /// <summary>
      /// </summary>
      /// <param name="propName"></param>
      /// <param name="value">true, if a value could be retrieved</param>
      /// <returns></returns>
      bool TryGetDefault( string propName, out object value );

      /// <summary>
      /// </summary>
      /// <param name="propName"></param>
      /// <param name="throwIfNull">if true, throws an exception when there is no value for propName</param>
      /// <returns></returns>
      object GetDefault( string propName, bool throwIfNull = false );

   }

}