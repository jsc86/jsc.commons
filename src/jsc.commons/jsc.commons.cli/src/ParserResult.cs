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
      private readonly Dictionary<IItem, IList<string>> _dynamicArguments;
      private readonly List<IFlag> _flags;

      private readonly List<IOption> _options;

      public ParserResult( ICliSpecification cliSpecification ) {
         CliSpecification = cliSpecification;
         _options = new List<IOption>( );
         _flags = new List<IFlag>( );
         _arguments = new Dictionary<IArgument, string>( );
         _dynamicArguments = new Dictionary<IItem, IList<string>>( );
      }

      private ParserResult(
            ICliSpecification cliSpecification,
            List<IOption> options,
            List<IFlag> flags,
            Dictionary<IArgument, string> arguments,
            Dictionary<IItem, IList<string>> dynamicArguments ) {
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

      public IEnumerable<string> GetDynamicArgumentList( IItem item ) {
         // todo: interface definition does not work as intended -> redesign
         throw new NotImplementedException( );
      }

      public ICliSpecification CliSpecification { get; }

      public object Clone( ) {
         return new ParserResult(
               CliSpecification,
               new List<IOption>( _options ),
               new List<IFlag>( _flags ),
               new Dictionary<IArgument, string>( _arguments ),
               new Dictionary<IItem, IList<string>>( _dynamicArguments ) );
      }

      internal void Set( IOption option ) {
         _options.Add( option );
      }

      internal void Set( IFlag flag ) {
         _flags.Add( flag );
      }

      internal void Set( IItem item ) {
         if( item is IOption )
            Set( (IOption)item );
         else if( item is IFlag )
            Set( (IFlag)item );
         else
            throw new Exception( $"{nameof( item )} must either be {nameof( IOption )} or {nameof( IFlag )}" );
      }

      internal void Unset( IOption option ) {
         _options.Remove( option );
      }

      internal void Unset( IFlag flag ) {
         _flags.Remove( flag );
      }

      internal void Unset( IItem item ) {
         if( item is IOption )
            Unset( (IOption)item );
         else if( item is IFlag )
            Unset( (IFlag)item );
         else
            throw new Exception( $"{nameof( item )} must either be {nameof( IOption )} or {nameof( IFlag )}" );
      }

      internal void SetArgument( IArgument argument, string value ) {
         _arguments[ argument ] = value;
      }

//      internal void SetArgument( IArgument argument, object value ){
//         _arguments[argument] = value;
//      }

      internal void RemoveArgument( IArgument argument ) {
         _arguments.Remove( argument );
      }

      internal void AddDynamicArgument( IItem item, string value ) {
         IList<string> dal;
         if( !_dynamicArguments.TryGetValue( item, out dal ) )
            _dynamicArguments[ item ] = dal = new List<string>( );
         dal.Add( value );
      }

      public override string ToString( ) {
         // TODO: this is fuggly, make it better
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