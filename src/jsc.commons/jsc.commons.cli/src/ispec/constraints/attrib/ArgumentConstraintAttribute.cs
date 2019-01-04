// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Reflection;

using jsc.commons.cli.ispec.constraints.interfaces;

namespace jsc.commons.cli.ispec.constraints.attrib {

   [AttributeUsage( AttributeTargets.Property|AttributeTargets.Class )]
   public abstract class ArgumentConstraintAttribute : Attribute {

      private IArgumentConstraintAttributeHandler _handler;

      public IArgumentConstraintAttributeHandler Handler {
         get {
            if( _handler == null ) {
               ArgumentConstraintAttributeHandlerAttribute caha =
                     GetType( ).GetCustomAttribute<ArgumentConstraintAttributeHandlerAttribute>( );
               if( caha == null )
                  throw new Exception(
                        $"{GetType( ).Name} has no {nameof( ArgumentConstraintAttributeHandlerAttribute )}" );

               _handler = (IArgumentConstraintAttributeHandler)Activator.CreateInstance( caha.Handler );
            }

            return _handler;
         }
      }

   }

}