// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;

using jsc.commons.cli.interfaces;
using jsc.commons.cli.policies;

namespace jsc.commons.cli.parser {

   public class CommandLineParser : ICommandLineParser {

      private readonly ICliSpecification _spec;

      public CommandLineParser( ICliSpecification spec ) {
         if( spec == null )
            throw new NullReferenceException( $"{nameof( spec )} must not be null" );

         new PolicyChecker( spec.Config ).Check( spec );

         _spec = spec;
      }

      public IParserResult Parse( string[] args ) {
         ParserResult pr = new ParserResult( _spec );
         IArgument expectedArg = _spec.Arguments.FirstOrDefault( );
         IItem expectedArgItem = null;
         foreach( string s in args )
            Parse( s, pr, ref expectedArg, ref expectedArgItem );

         return pr;
      }

      private void Parse(
            string s,
            ParserResult pr,
            ref IArgument expectedArg,
            ref IItem expectedArgItem ) {
         List<IParser> parsers = new List<IParser> {
               new OptionParser( _spec ),
               new FlagParser( _spec ),
               new ArgumentParser( expectedArg )
         };
         List<IParser> rem = new List<IParser>( );
         int index = -1;
         foreach( char c in s ) {
            index++;
            foreach( IParser parser in parsers )
               if( !parser.Next( c ) )
                  rem.Add( parser );

            if( rem.Count > 0 ) {
               rem.ForEach( p => parsers.Remove( p ) );
               rem.Clear( );
            }

            if( parsers.Count == 0 )
               throw new Exception( $"no parser matched input '{s}' at index {index}" );
         }

         IParser pMatch = parsers.FirstOrDefault( );
         if( pMatch is OptionParser optionParser )
            MatchOption( optionParser, pr, ref expectedArg, ref expectedArgItem );
         else if( pMatch is FlagParser parser )
            MatchFlag( parser, pr, ref expectedArg, ref expectedArgItem );
         else if( pMatch is ArgumentParser argumentParser )
            MatchArgument( argumentParser, pr, ref expectedArg, ref expectedArgItem );
      }

      private void MatchArgument(
            ArgumentParser pMatch,
            ParserResult pr,
            ref IArgument expectedArg,
            ref IItem expectedArgItem ) {
         pr.SetArgument( expectedArg, pMatch.Done( ) );
         SetNextExpectedArg( null, pr, ref expectedArg, ref expectedArgItem );
      }

      private void MatchFlag(
            FlagParser pMatch,
            ParserResult pr,
            ref IArgument expectedArg,
            ref IItem expectedArgItem ) {
         List<IItem> items = pMatch.Done( ).ToList( );
         foreach( IItem item in items )
            pr.Set( item );

         expectedArgItem = items.Last( );
         SetNextExpectedArg( items.Last( ), pr, ref expectedArg, ref expectedArgItem );
      }

      private void MatchOption(
            OptionParser pMatch,
            ParserResult pr,
            ref IArgument expectedArg,
            ref IItem expectedArgItem ) {
         IOption option = pMatch.Done( );
         pr.Set( option );
         expectedArgItem = option;
         SetNextExpectedArg( option, pr, ref expectedArg, ref expectedArgItem );
      }

      private void SetNextExpectedArg(
            IItem item,
            ParserResult pr,
            ref IArgument expectedArg,
            ref IItem expectedArgItem ) {
         List<IArgument> argList = item?.Arguments.ToList( )??_spec.Arguments.ToList( );
         if( item != null
               &&expectedArg != null
               &&_spec.Arguments.Contains( expectedArg ) )
            expectedArg = null;

         IArgument arg,
               prevArg = null;
         while( true ) {
            if( argList.Count == 0 ) {
               if( expectedArgItem != null ) {
                  expectedArg = expectedArgItem?.Arguments.FirstOrDefault( a => !pr.IsSet( a ) );
                  if( expectedArg == null )
                     expectedArgItem = null;
               }

               if( expectedArg == null )
                  expectedArg = _spec.Arguments.FirstOrDefault( a => !pr.IsSet( a ) );
               return;
            }

            arg = argList[ 0 ];
            if( expectedArg == null
                  ||prevArg == expectedArg ) {
               expectedArg = arg;
               return;
            }

            argList.RemoveAt( 0 );
            prevArg = arg;
         }
      }

   }

}