// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Castle.DynamicProxy.Internal;

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
               {typeof( bool? ), GetBoolArg},
               {typeof( double ), GetDoubleArg},
               {typeof( double? ), GetDoubleArg},
               {typeof( float ), GetFloatArg},
               {typeof( float? ), GetFloatArg},
               {typeof( long ), GetLongArg},
               {typeof( long? ), GetLongArg},
               {typeof( int ), GetIntArg},
               {typeof( int? ), GetIntArg},
               {typeof( FileInfo ), GetFileArg},
               {typeof( DirectoryInfo ), GetDirectoryArg}
         };
      }

      public ArgumentTypeMapper( Dictionary<Type, Func<ICliConfig, string, bool, Argument>> extendedMapping ) :
            this( ) {
         _extMapping = extendedMapping;
      }

      private static Argument GetStringArg( ICliConfig cliConfig, string name, bool optional ) {
         return new StringArg( name, null, optional );
      }

      private static Argument GetBoolArg( ICliConfig cliConfig, string name, bool optional ) {
         return new BoolArg( name, null, optional );
      }

      private static Argument GetDoubleArg( ICliConfig cliConfig, string name, bool optional ) {
         return new DoubleArg( name, null, cliConfig, optional );
      }

      private static Argument GetFloatArg( ICliConfig cliConfig, string name, bool optional ) {
         return new FloatArg( name, null, cliConfig, optional );
      }

      private static Argument GetLongArg( ICliConfig cliConfig, string name, bool optional ) {
         return new LongArg( name, null, cliConfig, optional );
      }

      private static Argument GetIntArg( ICliConfig cliConfig, string name, bool optional ) {
         return new IntArg( name, null, cliConfig, optional );
      }

      private static Argument GetFileArg( ICliConfig cliConfig, string name, bool optional ) {
         // TODO: issue with order of arguments: interactive set to true here is problematic
         return new FileArgument( name, null, optional );
      }

      private static Argument GetDirectoryArg( ICliConfig cliConfig, string name, bool optional ) {
         // same TODO as above (GetFileArg)
         return new DirectoryArgument( name, null, optional );
      }


      public IArgument Map( ArgumentAttribute argAttrib, PropertyInfo pi, ICliConfig config, string parent ) {
         Type argType = pi.PropertyType;
         if( argAttrib.Dynamic ) {
            if( !typeof( IEnumerable ).IsAssignableFrom( argType ) )
               throw new ArgumentException(
                     $"dynamic property type must be assignable to {typeof( IEnumerable<> )}  "
                     +$"but was {argType} for {pi.Name}" );
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

      public IArgument Map( FirstArgumentAttribute argAttrib, string name, Type t, ICliConfig config ) {
         if( argAttrib?.Dynamic??false ) {
            if( !typeof( IEnumerable ).IsAssignableFrom( t ) )
               throw new ArgumentException(
                     $"dynamic property type must be assignable to {typeof( IEnumerable<> )} but was {t} for {name}" );
            t = t.GenericTypeArguments[ 0 ];
         }

         Func<ICliConfig, string, bool, Argument> map = GetMap( t );

         Argument arg = map(
               config,
               argAttrib?.Name??name,
               true );

         arg.Description = argAttrib?.Description;

         return arg;
      }

      private Func<ICliConfig, string, bool, Argument> GetMap( Type t ) {
         Func<ICliConfig, string, bool, Argument> map;

         if( _extMapping != null
               &&_extMapping.TryGetValue( t, out map ) )
            return map;

         if( _mapping.TryGetValue( t, out map ) )
            return map;

         if( t.IsEnum ) {
            map = ( cliType, name, optional ) => MapEnumType( t, cliType, name, optional );
            return map;
         }

         if( t.IsNullableType( ) ) {
            Type tg = t.GenericTypeArguments[ 0 ];
            if( tg.IsEnum ) {
               map = ( cliType, name, optional ) => MapEnumType( tg, cliType, name, optional );
               return map;
            }
         }

         throw new Exception( $"no mapper for type '{t}'" );
      }

      private Argument MapEnumType( Type enumType, ICliConfig cliConfig, string name, bool optional ) {
         Type enumArgType = typeof( EnumArg<> ).MakeGenericType( enumType );
         Argument arg = (Argument)Activator.CreateInstance( enumArgType, name, (string)null, optional );
         return arg;
      }

   }

}