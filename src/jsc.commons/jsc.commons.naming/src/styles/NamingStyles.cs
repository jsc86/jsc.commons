// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.naming.interfaces;

// ReSharper disable InconsistentNaming

namespace jsc.commons.naming.styles {

   public static class NamingStyles {

      public static readonly INamingStyle camelCase = CamelCase.Instance;

      public static readonly INamingStyle PascalCase = styles.PascalCase.Instance;

      public static readonly INamingStyle snake_case = SnakeCase.Instance;

      public static readonly INamingStyle SNAKE_CASE = SnakeCaseUpper.Instance;

   }

}