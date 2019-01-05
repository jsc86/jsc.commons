// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.config.interfaces;

namespace jsc.commons.cli.ispec.constraints.interfaces {

   public interface IConstraintsProvider<T>
         where T : IConfiguration {

      void ProvideConstraints( IConstraintsProviderContext<T> cpc );

   }

}