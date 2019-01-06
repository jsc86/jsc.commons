// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.cli.ispec.attrib;
using jsc.commons.config.interfaces;

namespace jsc.commons.cli.tests.ispec {

   [CliDefinition( Description = "A great description of this CLI test interface." )]
   public interface ICliTest : IConfiguration {

      [Option( Flag = 'o', Description = "A very fine string option." )]
      [FirstArgument( Name = "stringArg", Description = "A nice string arg." )]
      string StringOptionOne { get; set; }

      [Argument( Of = nameof( StringOptionOne ), Dynamic = true, Description = "A dynamic int arg." )]
      IEnumerable<int> StringOptionOneDynIntArg { get; set; }

      [Option( Flag = 't', Description = "another fine string option" )]
      [FirstArgument( Name = "stringArg", Description = "A very nice string arg." )]
      string StringOptionTwo { get; set; }

      [Argument( Of = nameof( StringOptionTwo ), Description = "Some count argument.", Order = 2 )]
      int StringOptionTwoCount { get; set; }

      [Flag( Name = 'm', Description = "A fine flag." )]
      bool MyFlag { get; set; }

   }

}