// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.cli.interfaces;

namespace jsc.commons.cli.help {

   public abstract class HelpPrinterBase : IHelpPrinter {

      public HelpPrinterBase( ICliSpecification spec ) {
         if( spec == null )
            throw new ArgumentException( $"{nameof( spec )} must not be null", nameof( spec ) );

         Spec = spec;
      }

      protected ICliSpecification Spec { get; }

      public abstract void Print( );

   }

}