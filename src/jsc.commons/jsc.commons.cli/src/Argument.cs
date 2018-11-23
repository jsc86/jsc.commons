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
using jsc.commons.rc.generic.rules;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli {

   public abstract class Argument : IArgument {

      private string _description;

      static Argument( ) {
         RuleDeriver.Instance.AddArgumentRuleDeriver(
               typeof( Argument ),
               arg => {
                  if( ( (Argument)arg ).Optional )
                     return Enumerable.Empty<IRule<IParserResult>>( );

                  return new List<IRule<IParserResult>> {
                        ( (Argument)arg ).Parent == null
                              ? (IRule<IParserResult>)new ArgumentIsSet( arg )
                              : new Implies<IParserResult>(
                                    new ItemIsSet( ( (Argument)arg ).Parent ),
                                    new ArgumentIsSet( arg ) )
                  };
               } );
      }

      protected Argument( string name, bool optional = true ) {
         Name = name??string.Empty;
         Optional = optional;
      }

      internal IItem Parent { get; set; }

      public string Name { get; }

      public string Description {
         get => _description??string.Empty;
         protected internal set => _description = value;
      }

      public bool Optional { get; }

      public abstract Type ValueType { get; }

      public bool IsDynamicArgument { get; internal set; }

      public object Parse( string value, bool throwException = false ) {
         try {
            return ParseInternal( value );
         } catch( Exception ) {
            if( throwException )
               throw;
         }

         return null;
      }

      protected virtual object ParseInternal( string value ) {
         return value;
      }

   }

   public class Argument<T> : Argument, IArgument<T> {

      private object _defaultValue;

      static Argument( ) {
         RuleDeriver.Instance.AddArgumentRuleDeriver(
               typeof( Argument<T> ),
               arg => new List<IRule<IParserResult>> {
                     new ValidArgumentValue<T>( (Argument<T>)arg )
               } );
      }

      public Argument( string name, bool optional = true ) : base( name, optional ) { }

      public override Type ValueType => typeof( T );

      public T DefaultValue {
         get => (T)_defaultValue;
         protected set => _defaultValue = value;
      }

      public bool HasDefaultValue => _defaultValue != null;

   }

}