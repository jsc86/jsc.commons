// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections;
using System.Linq;
using System.Reflection;

using Castle.DynamicProxy.Internal;

using jsc.commons.cli.interfaces;
using jsc.commons.cli.ispec.attrib;
using jsc.commons.config.backend;
using jsc.commons.naming;

namespace jsc.commons.cli.ispec {

   public class ParserResultConfigBackend : ConfigBackendBase {

      private readonly IParserResult _pr;
      private readonly ICliSpecification _spec;

      private readonly ICliSpecDeriverConfig _specDeriverConfig;
      private readonly Type _t;

      public ParserResultConfigBackend(
            ICliSpecDeriverConfig specDeriverConfig,
            ICliSpecification spec,
            IParserResult pr,
            Type t ) {
         if( specDeriverConfig == null )
            throw new ArgumentException(
                  $"{nameof( specDeriverConfig )} must not be null",
                  nameof( specDeriverConfig ) );
         if( spec == null )
            throw new ArgumentException( $"{nameof( spec )} must not be null", nameof( spec ) );
         if( pr == null )
            throw new ArgumentException( $"{nameof( pr )} must not be null", nameof( pr ) );
         if( t == null )
            throw new ArgumentException( $"{nameof( t )} must not be null", nameof( t ) );

         _specDeriverConfig = specDeriverConfig;
         _spec = spec;
         _pr = pr;
         _t = t;
      }

      protected override bool Read( string key, Type type, out object value ) {
         PropertyInfo pi = _t.GetProperty( key );
         if( pi == null )
            throw new Exception( $"{_t.Name} has no property {key}" );

         return ReadOption( pi, out value )||
               ReadFlag( pi, out value )||
               ReadArgument( pi, type, out value )||
               ReadHelp( pi, out value );
      }

      private bool ReadHelp( PropertyInfo pi, out object value ) {
         value = null;
         HelpAttribute ha = pi.GetCustomAttribute<HelpAttribute>( );
         if( ha == null )
            return false;

         value = _pr.IsSet( _pr.CliSpecification.HelpOption );
         return true;
      }

      private bool ReadOption( PropertyInfo pi, out object value ) {
         value = null;
         OptionAttribute oa = pi.GetCustomAttribute<OptionAttribute>( );
         if( oa == null )
            return false;

         IOption opt = GetOptionForName( pi.Name );

         if( pi.PropertyType.IsAssignableFrom( typeof( bool ) ) ) {
            value = _pr.IsSet( opt );
            return true;
         }

         string argName = pi.GetCustomAttribute<FirstArgumentAttribute>( )?.Name??
               $"{pi.Name}{CliSpecDeriver.ImplicitFirstArg}";
         IArgument arg = opt.DynamicArgument != null
               &&opt.DynamicArgument.Name.Equals( argName )
                     ? opt.DynamicArgument
                     : opt.Arguments.FirstOrDefault( a => a.Name == argName );

         if( arg == null )
            throw new Exception( $"option {opt.Name} has no argument {argName}" );

         if( arg.IsDynamicArgument )
            value = GetValueForDynamicArgument( arg );
         else if( pi.PropertyType.IsNullableType( ) )
            value = GetValueForNullableArgument( arg, pi.PropertyType );
         else
            value = GetValueForArgument( arg );
         return true;
      }

      private bool ReadFlag( PropertyInfo pi, out object value ) {
         value = null;
         FlagAttribute fa = pi.GetCustomAttribute<FlagAttribute>( );
         if( fa == null )
            return false;

         IFlag flag = _spec.Flags.FirstOrDefault( f => f.Name.Equals( fa.Name ) );
         if( flag == null )
            throw new Exception(
                  string.Format(
                        "property {0} has no corresponding {1} with name {2}",
                        pi.Name,
                        nameof( IFlag ),
                        fa.Name ) );

         if( pi.PropertyType.IsAssignableFrom( typeof( bool ) ) ) {
            value = _pr.IsSet( flag );
            return true;
         }

         throw new Exception(
               string.Format(
                     "flag property {0} should be assignable from bool but is of type {1}",
                     pi.Name,
                     pi.PropertyType ) );
      }

      private bool ReadArgument( PropertyInfo pi, Type type, out object value ) {
         value = null;
         ArgumentAttribute aa = pi.GetCustomAttribute<ArgumentAttribute>( );
         if( aa == null )
            return false;

         IOption opt = null;
         if( !string.IsNullOrEmpty( aa.Of ) )
            opt = GetOptionForName( aa.Of );

         string argName = pi.Name;
         if( opt != null )
            argName = argName.Replace(
                  _specDeriverConfig.PropertyNamingStyle.ToString( opt.Name ),
                  string.Empty );

         IArgument arg = ( opt?.Arguments??_spec.Arguments ).FirstOrDefault( a => a.Name == argName )
               ??( _spec.DynamicArgument != null&&_spec.DynamicArgument.Name == argName
                     ? _spec.DynamicArgument
                     : _spec.Options.FirstOrDefault( o => o.DynamicArgument != null&&o.DynamicArgument.Name == argName )
                           ?.DynamicArgument );

         if( arg == null )
            throw new Exception( $"spec does not contain an {nameof( IArgument )} with name {argName}" );

         if( arg.IsDynamicArgument )
            value = GetValueForDynamicArgument( arg );
         else if( type.IsNullableType( ) )
            value = GetValueForNullableArgument( arg, type );
         else
            value = GetValueForArgument( arg );

         return true;
      }

      private object GetValueForNullableArgument( IArgument argument, Type type ) {
         object value = GetValueForArgument( argument );
         if( value == null )
            return null;

         return Activator.CreateInstance(
               type,
               value );
      }

      private object GetValueForDynamicArgument( IArgument argument ) {
         MethodInfo mi = GetType( )
               .GetMethod(
                     nameof( GetValueForDynamicArgumentGeneric ),
                     BindingFlags.Instance|BindingFlags.NonPublic )
               ?.MakeGenericMethod( argument.ValueType );

         return mi?.Invoke( this, new object[] {argument} );
      }

      private IEnumerable GetValueForDynamicArgumentGeneric<T1>( IArgument argument ) {
         return _pr.GetDynamicValues( (IArgument<T1>)argument );
      }

      private object GetValueForArgument( IArgument arg ) {
         if( _pr.IsSet( arg ) )
            return arg.Parse( _pr.GetValue( arg ), true );

         if( !arg.Optional )
            throw new Exception( $"non-optional argument {arg.Name} not set in parser result" );

         return null;
      }

      private IOption GetOptionForName( string name ) {
         UnifiedName optUnName = _specDeriverConfig.PropertyNamingStyle.FromString( name );
         IOption opt = _spec.Options.FirstOrDefault( o => o.Name.Equals( optUnName ) );
         if( opt == null )
            throw new Exception(
                  string.Format(
                        "property {0} has no corresponding {1} with unified name {2}",
                        name,
                        nameof( IOption ),
                        optUnName ) );
         return opt;
      }

      protected override void Save( string key, Type type, object value ) {
         throw new NotImplementedException( $"{nameof( ParserResultConfigBackend )} is readonly" );
      }

      protected override void BeforeRead( ) {
         // nop
      }

      protected override void AfterRead( ) {
         // nop
      }

      protected override void OnReadException( Exception exc ) {
         throw exc;
      }

      protected override void BeforeSave( ) {
         // nop
      }

      protected override void AfterSave( ) {
         // nop
      }

      protected override void OnSaveException( Exception exc ) {
         throw exc;
      }

   }

}