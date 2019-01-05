// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using jsc.commons.cli.interfaces;
using jsc.commons.cli.ispec.attrib;
using jsc.commons.cli.ispec.constraints;
using jsc.commons.cli.ispec.constraints.attrib;
using jsc.commons.config;
using jsc.commons.config.interfaces;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.ispec {

   public class CliSpecDeriver {

      internal const string ImplicitFirstArg = "__implicitFirstArg__";
      private readonly ArgumentTypeMapper _argTypeMapper;

      private readonly ICliSpecDeriverConfig _config;

      public CliSpecDeriver( ICliSpecDeriverConfig config = null ) {
         _config = config??Config.New<ICliSpecDeriverConfig>( );
         _argTypeMapper = new ArgumentTypeMapper( _config.ExtendedAttributeMappers );
      }

      internal static void CliSpecInterfaceCheck<T>( out Type t, out CliDefinitionAttribute cliDefAttrib )
            where T : IConfiguration {
         t = typeof( T );
         if( !t.IsInterface )
            throw new ArgumentException( $"{nameof( T )} has to be a interface" );

         cliDefAttrib =
               t.GetCustomAttributes<CliDefinitionAttribute>( ).FirstOrDefault( );

         if( cliDefAttrib == null )
            throw new ArgumentException( $"{nameof( T )} is not attributed with {nameof( CliDefinitionAttribute )}" );
      }

      public ICliSpecification DeriveSpecification<T>( ) where T : IConfiguration {
         CliSpecInterfaceCheck<T>( out Type t, out CliDefinitionAttribute cliDefAttrib );

         if( typeof( T ).GetProperties( ).Any( p => p.GetCustomAttribute<HelpAttribute>( ) != null )
               &&_config.CliConfig.AutoAddHelpOption == false )
            _config.CliConfig.AutoAddHelpOption = true;

         CliSpecification spec = new CliSpecification( _config.CliConfig ) {
               Description = cliDefAttrib.Description
         };

         AddOptions( spec, t );

         AddFlags( spec, t );

         AddArguments( spec, t );

         ConstraintsCollector<T> cc = new ConstraintsCollector<T>( _config, spec );
         foreach( IRule<IParserResult> rule in cc.Constraints )
            spec.Rules.Add( rule );

         if( spec.Options.Count+spec.Flags.Count+spec.Arguments.Count( ) == 0
               &&spec.DynamicArgument == null )
            throw new ArgumentException( $"{t.Name} does not define options, flags or arguments" );

         return spec;
      }

      private void AddArguments( CliSpecification spec, Type t ) {
         foreach( PropertyInfo pi in t.GetProperties( ) ) {
            ArgumentAttribute argAttrib = pi.GetCustomAttribute<ArgumentAttribute>( );
            if( argAttrib == null )
               continue;
            if( !string.IsNullOrEmpty( argAttrib.Of ) )
               continue;
            IArgument arg;
            if( argAttrib.Dynamic )
               spec.DynamicArgument = arg =
                     _argTypeMapper.Map(
                           argAttrib,
                           pi,
                           _config.CliConfig,
                           string.Empty );
            else
               spec.AddArgument(
                     arg = _argTypeMapper.Map(
                           argAttrib,
                           pi,
                           _config.CliConfig,
                           string.Empty ) );

            foreach( ArgumentConstraintAttribute ca in pi.GetCustomAttributes<ArgumentConstraintAttribute>( ) )
               ca.Handler.Handle( arg, ca );
         }
      }

      private void AddOptions( CliSpecification spec, Type t ) {
         foreach( PropertyInfo pi in t.GetProperties( ) ) {
            OptionAttribute optionAttrib = pi.GetCustomAttribute<OptionAttribute>( );
            if( optionAttrib == null )
               continue;
            AddOption( spec, t, optionAttrib, pi );
         }
      }

      private void AddOption(
            CliSpecification spec,
            Type t,
            OptionAttribute optionAttrib,
            PropertyInfo pi ) {
         Option opt = new Option(
               _config.PropertyNamingStyle.FromString( optionAttrib.Name??pi.Name ),
               optionAttrib.Optional,
               optionAttrib.Flag == 0? (char?)null : optionAttrib.Flag,
               GetDynamicArgument( spec, t, pi.Name ),
               ( pi.PropertyType.IsAssignableFrom( typeof( bool ) )
                     ? Enumerable.Empty<IArgument>( )
                     : pi.GetCustomAttribute<FirstArgumentAttribute>( )?.Dynamic??false
                           ? Enumerable.Empty<IArgument>( )
                           : new[] {
                                 _argTypeMapper.Map(
                                       pi.GetCustomAttribute<FirstArgumentAttribute>( ),
                                       $"{pi.Name}{ImplicitFirstArg}",
                                       pi.PropertyType,
                                       spec.Config )
                           } ).Union(
                     GetArguments( spec, t, pi.Name ) ) );
         opt.Description = optionAttrib.Description;
         spec.Options.Add( opt );
      }

      private void AddFlags( CliSpecification spec, Type t ) {
         foreach( PropertyInfo pi in t.GetProperties( ) ) {
            FlagAttribute flagAttrib = pi.GetCustomAttribute<FlagAttribute>( );
            if( flagAttrib == null )
               continue;
            AddFlag( spec, t, flagAttrib, pi );
         }
      }

      private void AddFlag(
            CliSpecification spec,
            Type t,
            FlagAttribute flagAttrib,
            PropertyInfo pi ) {
         Flag flag = new Flag(
               flagAttrib.Name,
               flagAttrib.Optional,
               GetDynamicArgument( spec, t, pi.Name ),
               GetArguments( spec, t, pi.Name ) );
         flag.Description = flagAttrib.Description;
         spec.Flags.Add( flag );
      }

      private IArgument GetDynamicArgument( CliSpecification spec, Type t, string parent ) {
         foreach( PropertyInfo pi in t.GetProperties( ) ) {
            ArgumentAttribute argAttrib = pi.GetCustomAttribute<ArgumentAttribute>( );
            FirstArgumentAttribute firstArgAttrib = pi.GetCustomAttribute<FirstArgumentAttribute>( );
            if( argAttrib == null
                  &&firstArgAttrib == null )
               continue;
            if( argAttrib != null
                  &&( string.Compare( parent, argAttrib.Of, StringComparison.InvariantCulture ) != 0
                        ||t.Name.Contains( parent ) ) )
               continue;
            if( firstArgAttrib != null
                  &&string.Compare( parent, pi.Name, StringComparison.InvariantCulture ) != 0 )
               continue;
            if( !argAttrib?.Dynamic??!firstArgAttrib.Dynamic )
               continue;

            if( argAttrib != null )
               return _argTypeMapper.Map(
                     argAttrib,
                     pi,
                     _config.CliConfig,
                     parent );

            return _argTypeMapper.Map(
                  firstArgAttrib,
                  $"{pi.Name}{ImplicitFirstArg}",
                  pi.PropertyType,
                  spec.Config );
         }

         return null;
      }

      private IEnumerable<IArgument> GetArguments( CliSpecification spec, Type t, string parent ) {
         List<IArgument> args = null;
         foreach( PropertyInfo pi in t.GetProperties( ) ) {
            ArgumentAttribute argAttrib = pi.GetCustomAttribute<ArgumentAttribute>( );
            if( argAttrib == null )
               continue;
            if( string.Compare( parent, argAttrib.Of, StringComparison.InvariantCulture ) != 0
                  ||t.Name.Contains( parent ) )
               continue;
            if( argAttrib.Dynamic )
               continue;
            args = args??new List<IArgument>( );
            AddArgument( args, argAttrib, pi, parent );
         }

         return args??Enumerable.Empty<IArgument>( );
      }

      private void AddArgument( List<IArgument> args, ArgumentAttribute argAttrib, PropertyInfo pi, string parent ) {
         args.Add(
               _argTypeMapper.Map(
                     argAttrib,
                     pi,
                     _config.CliConfig,
                     parent ) );
      }

   }

}