// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;

using jsc.commons.cli.config;
using jsc.commons.cli.interfaces;
using jsc.commons.naming;

namespace jsc.commons.cli {

   public class Option : ItemBase, IOption {

      private string _deUnifiedName;

      public Option(
            UnifiedName name,
            bool optional = true,
            char? flagAlias = null,
            IEnumerable<IArgument> args = null ) : this( name, optional, flagAlias, args?.ToArray( ) ) { }

      public Option(
            UnifiedName name,
            bool optional = true,
            char? flagAlias = null,
            params IArgument[] args ) : base( optional, args ) {
         if( name == null )
            throw new NullReferenceException( $"{nameof( name )} must not be null" );

         Name = name;
         FlagAlias = flagAlias;
      }

      public UnifiedName Name { get; }

      public string GetDeUnifiedName( ICliConfig conf ) {
         return _deUnifiedName = _deUnifiedName??conf.OptionNamingStyle.ToString( Name );
      }

      public char? FlagAlias { get; }

   }

}