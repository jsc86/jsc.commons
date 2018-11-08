// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Text;

using jsc.commons.cli.interfaces;

namespace jsc.commons.cli.parser {

   public class ArgumentParser : IParser<string> {

      private readonly IArgument _arg;

      private readonly StringBuilder _sb;

      public ArgumentParser( IArgument arg ) {
         _arg = arg;
         _sb = new StringBuilder( );
      }

      public bool Next( char c ) {
         _sb.Append( c );
         return true;
      }

      public string Done( ) {
         // TODO: implement type check (not necessarily here)
         return _sb.ToString( );
      }

   }

}