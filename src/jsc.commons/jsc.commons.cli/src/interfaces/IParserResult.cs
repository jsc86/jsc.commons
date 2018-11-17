// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

namespace jsc.commons.cli.interfaces {

   public interface IParserResult : ICloneable {

      ICliSpecification CliSpecification { get; }

      bool IsSet( IItem item );

      bool IsSet( IArgument argument );

      string GetValue( IArgument argument );

      T GetValue<T>( IArgument<T> argument );

      IEnumerable<T> GetDynamicValues<T>( IArgument<T> dynamicArgument );

   }

}