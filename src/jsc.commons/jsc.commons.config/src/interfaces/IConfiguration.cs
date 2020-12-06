// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

namespace jsc.commons.config.interfaces {

   /// <summary>
   ///    A Configuration provides facilities to get and set values for given
   ///    keys and read and write them from and to a Configuration Backend.
   ///    This interface is meant to be only inherited by other interfaces;
   ///    never to be manually implemented by a class. Inheriting interfaces
   ///    should be used with the Config-class New-mehtod to create a
   ///    dynamic proxy providing access to all basic configuration features
   ///    and direct and type safe access to configuration properties
   ///    defined in an inheriting interface.
   /// </summary>
   public interface IConfiguration {

      /// <summary>
      ///    A Backend to read and write configuration properties from and to.
      /// </summary>
      IConfigurationBackend Backend { get; set; }

      /// <summary>
      /// </summary>
      /// <param name="key"></param>
      /// <param name="type">
      ///    type is optional and works as a constraint to make sure
      ///    the configuration value has the expected type
      /// </param>
      object this[ string key, Type type = null ] { get; set; }

      /// <summary>
      ///    The set of keys for all configuration properties contained by this Configuration.
      /// </summary>
      IEnumerable<string> Keys { get; }

      /// <summary>
      ///    Save all configuration properties to the set Backend.
      /// </summary>
      void Save( );

      /// <summary>
      ///    Read all configuration properties from the set Backend.
      /// </summary>
      void Read( bool skipNullValues = false );

      Tuple<object, Type> GetConfigProperty( string key );

   }

}