// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.behaving;
using jsc.commons.behaving.interfaces;
using jsc.commons.cli.config;
using jsc.commons.cli.interfaces;
using jsc.commons.cli.parser;
using jsc.commons.config.interfaces;

namespace jsc.commons.cli.ispec {

   public class InterfaceSpecBoilerPlateHelper<T> where T : IConfiguration {

      private readonly string[] _args;
      private ICliConfig _cliConfig;
      private T _cliConfigObject;
      private CliSpecDeriver _cliSpecDeriver;
      private ICliSpecDeriverConfig _cliSpecDeriverConfig;
      private ICliSpecification _cliSpecification;
      private ICommandLineParser _commandLineParser;
      private IInterfaceSpecBoilerPlateHelperConfig _config;
      private ConflictResolver _conflictResolver;
      private IBehaviors _conflictResolverContext;
      private IParserResult _parserResult;
      private ParserResultMapper _parserResultMapper;
      private IParserResult _parserResultResolved;

      public InterfaceSpecBoilerPlateHelper( string[] args ) {
         if( args == null )
            throw new ArgumentException( $"{nameof( args )} must not be null", nameof( args ) );

         _args = args;
      }

      public IInterfaceSpecBoilerPlateHelperConfig Config =>
            _config??( _config = commons.config.Config.New<IInterfaceSpecBoilerPlateHelperConfig>( ) );

      public ICliConfig CliConfig => _cliConfig??( _cliConfig = commons.config.Config.New<ICliConfig>( ) );

      public ICliSpecDeriverConfig CliSpecDeriverConfig {
         get {
            if( _cliSpecDeriverConfig == null ) {
               _cliSpecDeriverConfig = commons.config.Config.New<ICliSpecDeriverConfig>( );
               _cliSpecDeriverConfig.CliConfig = CliConfig;
            }

            return _cliSpecDeriverConfig;
         }
      }

      public ICliSpecification CliSpecification =>
            _cliSpecification??( _cliSpecification = CliSpecDeriver.DeriveSpecification<T>( ) );

      public CliSpecDeriver CliSpecDeriver =>
            _cliSpecDeriver??( _cliSpecDeriver = new CliSpecDeriver( CliSpecDeriverConfig ) );

      public ConflictResolver ConflictResolver => _conflictResolver
            ??( _conflictResolver = new ConflictResolver( CliSpecification, Config.UserPrompt ) );

      public IParserResult ParserResult => _parserResult??( _parserResult = CommandLineParser.Parse( _args ) );

      public IBehaviors ConflictResolverContext =>
            _conflictResolverContext??( _conflictResolverContext = new BehaviorsBase( ) );

      public IParserResult ParserResultResolved {
         get {
            if( _parserResultResolved == null )
               if( !ConflictResolver.Resolve(
                     ParserResult,
                     out _parserResultResolved,
                     ConflictResolverContext ) )
                  throw new Exception( "unresolved conflicts" );
            return _parserResultResolved;
         }
      }

      public ICommandLineParser CommandLineParser =>
            _commandLineParser??( _commandLineParser = new CommandLineParser( CliSpecification ) );

      public ParserResultMapper ParserResultMapper => _parserResultMapper
            ??( _parserResultMapper = new ParserResultMapper( CliSpecDeriverConfig, CliSpecification ) );

      public T CliConfigObject {
         get {
            if( _cliConfigObject == null )
               _cliConfigObject = ParserResultMapper.Map<T>( ParserResultResolved );

            return _cliConfigObject;
         }
      }

   }

}