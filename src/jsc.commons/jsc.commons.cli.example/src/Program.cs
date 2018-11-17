// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.behaving;
using jsc.commons.behaving.interfaces;
using jsc.commons.cli.arguments;
using jsc.commons.cli.config;
using jsc.commons.cli.interfaces;
using jsc.commons.cli.parser;
using jsc.commons.cli.rules;
using jsc.commons.config;
using jsc.commons.naming;
using jsc.commons.rc.generic.rules;

namespace jsc.commons.cli.example {

   internal class Program {

      public static void Main( string[] args ) {
         args = "-a".Split( ' ' );
         ICliConfig conf = Config.New<ICliConfig>( );

         LongArg arg1 = new LongArg( "arg1", conf, 1, 10, false );
         BoolArg arg2 = new BoolArg( "arg2" );
         StringArg arg3 = new StringArg( "arg3" );

         IOption opt1 = new Option(
               new UnifiedName( "opt", "one" ),
               false,
               'a',
               null,
               arg1 );

         IOption opt2 = new Option(
               new UnifiedName( "opt", "two" ),
               true,
               'b',
               null,
               arg2 );

         IOption opt3 = new Option(
               new UnifiedName( "opt", "three" ),
               true,
               'c',
               null,
               arg3 );

         CliSpecification spec = new CliSpecification( conf );
         spec.Options.Add( opt1 );
         spec.Options.Add( opt2 );
         spec.Options.Add( opt3 );
//         spec.Flags.Add( new Flag( 'b' ) );

         spec.Rules.Add( new Implies<IParserResult>( new ItemIsSet( opt1 ), new ItemIsSet( opt2 ) ) );

         ICommandLineParser clp = new CommandLineParser( spec );
         ConflictResolver cr = new ConflictResolver( spec );

         IParserResult pr = clp.Parse( args );
         IBehaviors context = new BehaviorsBase( );
         context.Set( new ConfigBehavior( conf ) );
         if( cr.Resolve( pr, out pr, context ) ) {
            Console.WriteLine( pr );
            Console.WriteLine( "cool" );
         } else {
            Console.WriteLine( "nope" );
         }
      }

   }

}