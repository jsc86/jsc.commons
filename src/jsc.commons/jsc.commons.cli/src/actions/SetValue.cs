// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;

using jsc.commons.behaving.interfaces;
using jsc.commons.cli.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.actions {

   public class SetValue<T> : IAction<IParserResult> {

      private string _description;

      public SetValue( IArgument<T> target, T value ) {
         Target = target;
         Value = value;
      }

      public IArgument<T> Target { get; }

      public T Value { get; }

      public void Apply( IParserResult subject, IBehaviors context ) {
         ( subject as ParserResult )?.SetArgument( Target, Value.ToString( ) );
      }

      public bool Contradicts( IAction<IParserResult> a ) {
         return a is PromptValue<T> promptVal&&promptVal.Target == Target||
               a is SetValue<T> setVal&&setVal.Target == Target;
      }

      public bool ChangesSubject( IParserResult subject ) {
         return Comparer<T>.Default.Compare(
                     Value,
                     subject.GetValue( Target ) )
               != 0;
      }

      public string Description => _description ??= $"set argument {Target.Name} to {Value}";

      public bool IsInteractive { get; } = false;

      internal static TX Add<TX>( TX a, TX b ) where TX : struct {
         return ExecOperator( "op_Addition", a, b );
      }

      internal static TX Sub<TX>( TX a, TX b ) where TX : struct {
         return ExecOperator( "op_Subtraction", a, b );
      }

      private static TX ExecOperator<TX>( string op, TX a, TX b ) where TX : struct {
         // ReSharper disable once PossibleNullReferenceException
         return (TX)a.GetType( )
               .GetMethod( op )
               .Invoke(
                     null,
                     new object[] {
                           a,
                           b
                     } );
      }

   }

}