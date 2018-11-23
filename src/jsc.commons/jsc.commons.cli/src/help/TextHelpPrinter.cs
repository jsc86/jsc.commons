// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.IO;
using System.Linq;

using jsc.commons.cli.interfaces;
using jsc.commons.config;

namespace jsc.commons.cli.help {

   public class TextHelpPrinter : HelpPrinterBase {

      private readonly ITextHelpPrinterConfig _conf;

      private readonly TextWriter _writer;

      public TextHelpPrinter(
            ICliSpecification spec,
            ITextHelpPrinterConfig conf = null ) : base( spec ) {
         _conf = conf??Config.New<ITextHelpPrinterConfig>( );
         _writer = _conf.TextWriter;
      }

      public override void Print( ) {
         if( !string.IsNullOrEmpty( Spec.Description ) )
            _writer.WriteLine( Spec.Description );

         GetSortedOptions( ).ForEach( PrintOption );

         GetSortedFlags( ).ForEach( PrintFlag );

         foreach( IArgument arg in GetArguments( Spec ) ) {
            _writer.WriteLine( );
            PrintArgument( arg, false );
         }
      }

      private void Indent( int spaces ) {
         while( spaces-- > 0 )
            _writer.Write( ' ' );
      }

      private void PrintOption( IOption opt ) {
         List<IArgument> args = opt.Arguments.ToList( );

         Indent( 2 );
         if( opt.FlagAlias.HasValue ) {
            _writer.Write( Spec.Config.FlagPrefix( ) );
            _writer.Write( opt.FlagAlias );
            _writer.Write( ',' );
         } else {
            Indent( Spec.Config.FlagPrefix( ).Length+2 );
         }

         Indent( 1 );
         _writer.Write( Spec.Config.OptionPrefix( ) );
         _writer.Write( opt.GetDeUnifiedName( Spec.Config ) );
         Indent( 1 );
         PrintArgumentLine( args );
         PrintDynamicArgumentLine( opt );
         _writer.WriteLine( );

         if( !string.IsNullOrEmpty( opt.Description ) ) {
            Indent( 15 );
            _writer.WriteLine( opt.Description );
         }

         if( opt.DynamicArgument != null )
            args.Add( opt.DynamicArgument );

         foreach( IArgument arg in args )
            PrintArgument( arg, true );
      }

      private void PrintDynamicArgumentLine( IItem item ) {
         if( item.DynamicArgument == null )
            return;
         IArgument dynArg = item.DynamicArgument;
         Indent( 1 );
         _writer.Write( dynArg.Name );
         _writer.Write( " [" );
         _writer.Write( dynArg.Name );
         _writer.Write( " [" );
         _writer.Write( dynArg.Name );
         _writer.Write( " [...]]]" );
      }

      private void PrintFlag( IFlag flag ) {
         List<IArgument> args = flag.Arguments.ToList( );

         Indent( 2 );
         _writer.Write( Spec.Config.FlagPrefix( ) );
         _writer.Write( flag.Name );

         Indent( 1 );
         PrintArgumentLine( args );
         _writer.WriteLine( );

         if( !string.IsNullOrEmpty( flag.Description ) ) {
            Indent( 15 );
            _writer.WriteLine( flag.Description );
         }

         foreach( IArgument arg in args )
            PrintArgument( arg, true );
      }

      private void PrintArgument( IArgument arg, bool subArg ) {
         if( string.IsNullOrEmpty( arg.Description ) )
            return;
         Indent( subArg? 10 : 2 );
         _writer.Write( arg.Name );
         _writer.Write( ": " );
         _writer.WriteLine( arg.Description );
      }

      private void PrintArgumentLine( IEnumerable<IArgument> args ) {
         int i = 0;
         foreach( IArgument arg in args ) {
            _writer.Write( ' ' );
            if( arg.Optional ) {
               _writer.Write( '[' );
               i++;
            }

            _writer.Write( arg.Name );
         }

         while( i-- > 0 )
            _writer.Write( ']' );
      }

      private List<IOption> GetSortedOptions( ) {
         List<IOption> sortedOpts = Spec.Options.ToList( );
         sortedOpts.Sort( ( opt1, opt2 ) => opt1.Name.CompareTo( opt2.Name ) );
         return sortedOpts;
      }

      private List<IFlag> GetSortedFlags( ) {
         List<IFlag> sortedFlags = Spec.Flags.ToList( );
         sortedFlags.Sort( ( flag1, flag2 ) => flag1.Name.CompareTo( flag2.Name ) );
         return sortedFlags;
      }

      private List<IArgument> GetArguments( IItem item ) {
         return item.Arguments.ToList( );
      }

   }

}