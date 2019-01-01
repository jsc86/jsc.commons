// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.cli.interfaces;

namespace jsc.commons.cli.ispec.attrib.constraints {

   public interface IArgumentConstraintAttributeHandler {

      void Handle( IArgument arg, ArgumentConstraintAttribute attrib );

   }

}