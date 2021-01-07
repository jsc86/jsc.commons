// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.hierarchy.path;

namespace jsc.commons.hierarchy {

   public class Trace {

      public const string ActionGet = "GET";
      public const string ActionSet = "SET";
      public const string ActionDelete = "DEL";
      public const string ActionMove = "MOV";
      public const string ActionList = "LST";

      public const string HintBegin = "BEG";
      public const string HintEnd = "END";
      public const string HintError = "ERR";

   }

   public delegate void TraceHandler( string module, string action, string hint, params Path[] paths );

}
