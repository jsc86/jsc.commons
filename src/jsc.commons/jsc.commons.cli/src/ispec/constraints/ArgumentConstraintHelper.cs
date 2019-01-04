// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.cli.interfaces;
using jsc.commons.cli.ispec.constraints.attrib;

namespace jsc.commons.cli.ispec.constraints {

   public static class ArgumentConstraintHelper {

      public static void XHandle<TArg, TAttr>(
            IArgument arg,
            ArgumentConstraintAttribute attrib,
            Action<TArg, TAttr> action )
            where TArg : IArgument
            where TAttr : ArgumentConstraintAttribute {
         if( arg == null )
            throw new ArgumentNullException( $"{nameof( arg )} must not be null", nameof( arg ) );
         if( !( arg is TArg ) )
            throw new ArgumentException( $"{nameof( arg )} must be assignable to {nameof( TArg )}", nameof( arg ) );
         if( attrib == null )
            throw new ArgumentNullException( $"{nameof( attrib )} must not be null", nameof( attrib ) );
         if( !( attrib is TAttr ) )
            throw new ArgumentException(
                  $"{nameof( attrib )} must ne assignable to {nameof( TAttr )}",
                  nameof( attrib ) );
         if( action == null )
            throw new ArgumentNullException( $"{nameof( action )} must not be null", nameof( action ) );

         action( (TArg)arg, (TAttr)attrib );
      }

   }

}