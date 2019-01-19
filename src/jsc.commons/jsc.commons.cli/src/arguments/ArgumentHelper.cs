// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.cli.config;

namespace jsc.commons.cli.arguments {

   internal static class ArgumentHelper {

      public static bool SignedNumberCanStartWithPrefixCheck( ICliConfig conf ) {
         return "-".Equals( conf.FlagPrefix( ) )||"-".Equals( conf.OptionPrefix );
      }

   }

}