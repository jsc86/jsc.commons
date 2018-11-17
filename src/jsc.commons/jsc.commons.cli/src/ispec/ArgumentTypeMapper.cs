// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using jsc.commons.cli.arguments;
using jsc.commons.cli.config;
using jsc.commons.cli.interfaces;
using jsc.commons.cli.ispec.attrib;

namespace jsc.commons.cli.ispec {

   internal class ArgumentTypeMapper {

      private readonly Dictionary<Type, Func<ICliConfig, string, bool, Argument>> _extMapping;

      private readonly Dictionary<Type, Func<ICliConfig, string, bool, Argument>> _mapping;

      private ArgumentTypeMapper( ) {
         _mapping = new Dictionary<Type, Func<ICliConfig, string, bool, Argument>> {
               {typeof( string ), GetStringArg},
               {typeof( bool ), GetBoolArg},
               {typeof( double ), GetDoubleArg},
               {typeof( float ), GetDoubleArg},
               {typeof( long ), GetLongArg},
               {typeof( int ), GetLongArg}
         };
      }

      public ArgumentTypeMapper( Dictionary<Type, Func<ICliConfig, string, bool, Argument>> extendedMapping ) :
            this( ) {
         _extMapping = extendedMapping;
      }

      private static Argument GetStringArg( ICliConfig cliConfig, string name, bool optional ) {
         return new StringArg( name, optional );
      }

      private static Argument GetBoolArg( ICliConfig cliConfig, string name, bool optional ) {
         return new BoolArg( name, optional );
      }

      private static Argument GetDoubleArg( ICliConfig cliConfig, string name, bool optional ) {
         return new DoubleArg( name, cliConfig, optional );
      }

      private static Argument GetLongArg( ICliConfig cliConfig, string name, bool optional ) {
         return new LongArg( name, cliConfig, optional );
      }

      public IArgument Map( ArgumentAttribute argAttrib, PropertyInfo pi, ICliConfig config, string parent ) {
         Type argType = pi.PropertyType;
         if( argAttrib.Dynamic ) {
            if( !typeof( IEnumerable ).IsAssignableFrom( argType ) )
               throw new ArgumentException(
                     string.Format(
                           "dynamic property type must be assignable to {0} but was {1} for {2}",
                           typeof( IEnumerable<> ),
                           argType,
                           pi.Name ) );
            argType = argType.GenericTypeArguments[ 0 ];
         }

         Func<ICliConfig, string, bool, Argument> map = GetMap( argType );

         Argument arg = map(
               config,
               argAttrib.Name??( string.IsNullOrEmpty( parent )? pi.Name : pi.Name.Replace( parent, string.Empty ) ),
               argAttrib.Optional );

         arg.Description = argAttrib.Description;

         return arg;
      }

      public IArgument Map( string name, Type t, ICliConfig config ) {
         Func<ICliConfig, string, bool, Argument> map = GetMap( t );

         Argument arg = map(
               config,
               name,
               false );

         return arg;
      }

      private Func<ICliConfig, string, bool, Argument> GetMap( Type t ) {
         Func<ICliConfig, string, bool, Argument> map;
         if( _extMapping == null
               ||!_extMapping.TryGetValue( t, out map ) )
            if( !_mapping.TryGetValue( t, out map ) )
               throw new Exception( $"no mapper for type '{t}'" );
         return map;
      }

   }

}