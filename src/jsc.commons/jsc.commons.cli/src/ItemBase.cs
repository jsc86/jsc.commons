// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;

using jsc.commons.cli.interfaces;
using jsc.commons.cli.rules;
using jsc.commons.misc;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli {

   public class ItemBase : IItem {

      private readonly misc.Lazy<IList<IArgument>> _lazyArguments =
            new misc.Lazy<IList<IArgument>>(
                  ( ) => new List<IArgument>( ) );

      private string _description;

      private IArgument _dynamicArgument;

      static ItemBase( ) {
         RuleDeriver.Instance.AddItemRuleDeriver(
               typeof( ItemBase ),
               item => {
                  if( item.Optional )
                     return Enumerable.Empty<IRule<IParserResult>>( );

                  return new List<IRule<IParserResult>> {
                        new ItemIsSet( item )
                  };
               } );
      }

      protected ItemBase( bool optional, IArgument dynamicArgument, IEnumerable<IArgument> args ) : this(
            optional,
            dynamicArgument,
            args.ToArray( ) ) { }

      protected ItemBase( bool optional, IArgument dynamicArgument, params IArgument[] args ) {
         Optional = optional;

         DynamicArgument = dynamicArgument;

         if( args == null )
            return;

         foreach( IArgument arg in args )
            AddArgument( arg );
      }

      public string Description {
         get => _description??string.Empty;
         protected internal set => _description = value;
      }

      public IEnumerable<IArgument> Arguments => _lazyArguments.IsInitialized
            ? new EnumerableWrapper<IArgument>( _lazyArguments.Instance )
            : Enumerable.Empty<IArgument>( );

      public IArgument DynamicArgument {
         get => _dynamicArgument;
         internal set {
            if( value != null
                  &&!( value is Argument ) )
               throw new ArgumentException(
                     $"{nameof( value )} must derive from {nameof( Argument )}",
                     nameof( value ) );

            if( _dynamicArgument != null )
               ( (Argument)_dynamicArgument ).IsDynamicArgument = false;

            if( value != null )
               ( (Argument)value ).IsDynamicArgument = true;
            _dynamicArgument = value;
         }
      }

      public bool Optional { get; }

      internal void AddArgument( IArgument arg ) {
         if( arg == null )
            throw new NullReferenceException( $"{nameof( arg )} must not be null" );

         _lazyArguments.Instance.Add( arg );
         if( arg is Argument argument )
            argument.Parent = this;
      }

   }

}