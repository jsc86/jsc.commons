// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.cli.config;

namespace jsc.commons.cli.interfaces {

   public interface IArgument {

      string Name { get; }

      string Description { get; }

      bool Optional { get; }

      Type ValueType { get; }

      bool IsDynamicArgument { get; }

      object Parse( string value, bool throwException = false );

      bool CanStartWithPrefix( ICliConfig conf );

   }

   public interface IArgument<T> : IArgument {

      T DefaultValue { get; }

      bool HasDefaultValue { get; }

   }

}