// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.behaving.interfaces;

namespace jsc.commons.cli.config {

   public class ConfigBehavior : IBehavior {

      public ConfigBehavior( ICliConfig config ) {
         Config = config;
      }

      public ICliConfig Config { get; }

   }

}