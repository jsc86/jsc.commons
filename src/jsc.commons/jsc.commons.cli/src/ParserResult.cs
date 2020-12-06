// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;

using jsc.commons.cli.interfaces;

namespace jsc.commons.cli {

   public class ParserResult : IParserResult {

      private readonly Dictionary<IArgument, string> _arguments;
      private readonly Dictionary<IArgument, List<string>> _dynamicArguments;
      private readonly List<IFlag> _flags;

      private readonly List<IOption> _options;

      public ParserResult( ICliSpecification cliSpecification ) {
         CliSpecification = cliSpecification;
         _options = new List<IOption>( );
         _flags = new List<IFlag>( );
         _arguments = new Dictionary<IArgument, string>( );
         _dynamicArguments = new Dictionary<IArgument, List<string>>( );
      }

      private ParserResult(
            ICliSpecification cliSpecification,
            List<IOption> options,
            List<IFlag> flags,
            Dictionary<IArgument, string> arguments,
            Dictionary<IArgument, List<string>> dynamicArguments ) {
         CliSpecification = cliSpecification;
         _options = options;
         _flags = flags;
         _arguments = arguments;
         _dynamicArguments = dynamicArguments;
      }

      public bool IsSet( IItem item ) {
         return _options.Any( o => o == item )||
               _flags.Any( f => f == item );
      }

      public bool IsSet( IArgument argument ) {
         return _arguments.ContainsKey( argument );
      }

      public string GetValue( IArgument argument ) {
         return _arguments[ argument ];
      }

      public T GetValue<T>( IArgument<T> argument ) {
         return (T)argument.Parse( _arguments[ argument ] );
      }

      public IEnumerable<T> GetDynamicValues<T>( IArgument<T> dynamicArgument ) {
         return !_dynamicArguments.TryGetValue( dynamicArgument, out List<string> dal )
               ? Enumerable.Empty<T>( )
               : dal.ConvertAll( s => Convert<T>( s, dynamicArgument ) );
      }

      public ICliSpecification CliSpecification { get; }

      public object Clone( ) {
         return new ParserResult(
               CliSpecification,
               new List<IOption>( _options ),
               new List<IFlag>( _flags ),
               new Dictionary<IArgument, string>( _arguments ),
               new Dictionary<IArgument, List<string>>( _dynamicArguments ) );
      }

      private T Convert<T>( string str, IArgument arg ) {
         object o = arg.Parse( str, true );
         if( o is T value )
            return value;
         return (T)System.Convert.ChangeType( o, typeof( T ) );
      }

      internal void Set( IOption option ) {
         _options.Add( option );
      }

      internal void Set( IFlag flag ) {
         _flags.Add( flag );
      }

      internal void Set( IItem item ) {
         switch( item ) {
            case IOption option:
               Set( option );
               break;
            case IFlag flag:
               Set( flag );
               break;
            default:
               throw new Exception( $"{nameof( item )} must either be {nameof( IOption )} or {nameof( IFlag )}" );
         }
      }

      internal void Unset( IOption option ) {
         _options.Remove( option );
      }

      internal void Unset( IFlag flag ) {
         _flags.Remove( flag );
      }

      internal void Unset( IItem item ) {
         switch( item ) {
            case IOption option:
               Unset( option );
               break;
            case IFlag flag:
               Unset( flag );
               break;
            default:
               throw new Exception( $"{nameof( item )} must either be {nameof( IOption )} or {nameof( IFlag )}" );
         }
      }

      internal void SetArgument( IArgument argument, string value ) {
         _arguments[ argument ] = value;
      }

      internal void RemoveArgument( IArgument argument ) {
         _arguments.Remove( argument );
      }

      internal void AddDynamicArgument( IArgument argument, string value ) {
         if( !_dynamicArguments.TryGetValue( argument, out List<string> dal ) )
            _dynamicArguments[ argument ] = dal = new List<string>( );

         dal.Add( value );
      }

      public override string ToString( ) {
         // TODO: this is fuggly, make it better (https://github.com/jsc86/jsc.commons/issues/7)
         return string.Format(
               "{0} {1} {2}",
               string.Join(
                     " ",
                     _options.ConvertAll(
                           opt => string.Format(
                                 "{0}{1} {2}",
                                 CliSpecification.Config.OptionPrefix( ),
                                 opt.GetDeUnifiedName( CliSpecification.Config ),
                                 string.Join( " ", opt.Arguments.Where( IsSet ).Select( GetValue ) ) ) ) ),
               string.Join(
                     " ",
                     _flags.ConvertAll(
                           flag => string.Format(
                                 "{0}{1} {2}",
                                 CliSpecification.Config.FlagPrefix( ),
                                 flag.Name,
                                 string.Join( " ", flag.Arguments.Where( IsSet ).Select( GetValue ) ) ) ) ),
               string.Join(
                     " ",
                     _arguments.Keys.Where( arg => ( arg as Argument )?.Parent == null&&IsSet( arg ) )
                           .Select( GetValue ) ) );
      }

   }

}