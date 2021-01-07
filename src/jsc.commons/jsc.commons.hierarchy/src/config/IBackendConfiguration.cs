// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.config;
using jsc.commons.config.interfaces;
using jsc.commons.hierarchy.backend.interfaces;

namespace jsc.commons.hierarchy.config {

   [Config]
   public interface IBackendConfiguration : IConfiguration {

      [ConfigValue]
      Type BackendType { get; set; }

      [ConfigValue]
      Func<IHierarchyConfiguration, IBackendConfiguration, IHierarchyBackend> BackendFactory { get; set; }

      [ConfigValue]
      TraceHandler TraceHandler { get; set; }

   }

}