// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Linq;

using jsc.commons.behaving.interfaces;
using jsc.commons.cli.help;
using jsc.commons.cli.interfaces;
using jsc.commons.cli.rules;
using jsc.commons.rc;
using jsc.commons.rc.generic;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli {

   public class ConflictResolver {

      private static readonly IEnumerable<ISolution<IParserResult>> __defaultSolutions;
      private static readonly Solution<IParserResult> __cancelSolution;
      private static readonly Solution<IParserResult> __autoSolveSolution;

      private readonly IEnumerable<IRule<IParserResult>> _rules;
      private readonly Func<IList<ISolution<IParserResult>>, ISolution<IParserResult>> _userPrompt;

      static ConflictResolver( ) {
         __cancelSolution = new Solution<IParserResult>(
               new[] {
                     new GenericAction<IParserResult>( "cancel" )
               } );
         __autoSolveSolution = new Solution<IParserResult>(
               new[] {
                     new GenericAction<IParserResult>( "auto solve" )
               } );
         __defaultSolutions = new List<ISolution<IParserResult>> {
               __autoSolveSolution,
               __cancelSolution
         };
      }

      public ConflictResolver(
            ICliSpecification spec = null,
            Func<IList<ISolution<IParserResult>>, ISolution<IParserResult>> userPrompt = null ) {
         _rules = spec?.Rules??Enumerable.Empty<IRule<IParserResult>>( );
         _userPrompt = userPrompt??UserPrompt;
      }

      public bool Resolve( IParserResult prIn, out IParserResult prOut, IBehaviors context ) {
         IEnumerable<IRule<IParserResult>> rules =
               _rules.Union(
                           RuleDeriver.Instance.DeriveRules( prIn.CliSpecification ) )
                     .ToList( );

         HelpOption hlpOpt = (HelpOption)prIn.CliSpecification.Options.FirstOrDefault( opt => opt is HelpOption );
         if( hlpOpt != null )
            rules = rules.Select(
                  r => new HelpIsSetOr( hlpOpt, r ) );

         RuleCheckerBase<IParserResult> rc = new RuleCheckerBase<IParserResult>( rules );

         if( rc.Check( prIn, context ) == NonViolation<IParserResult>.Instance ) {
            prOut = prIn;
            return true;
         }

         IViolation<IParserResult> violation;
         prOut = (IParserResult)prIn.Clone( );
         while( ( violation = rc.Check( prOut, context ) ) != NonViolation<IParserResult>.Instance ) {
            // TODO: replace Console.WriteLine with a more abstract/generic output method (https://github.com/jsc86/jsc.commons/issues/8)
            Console.WriteLine( );
            Console.WriteLine( prOut );
            Console.WriteLine( $"> {violation.Description}" );
            if( !violation.HasSolution ) {
               Console.WriteLine( "> conflict has no solution" );
               prOut = null;
               return false;
            }

            switch( Prompt( violation.Solutions, out ISolution<IParserResult> chosenSolution ) ) {
               case UserAction.Cancel:
                  prOut = null;
                  return false;
               case UserAction.AutoSolve:
                  AutoSolve( prOut, out prOut, rules, context );
                  break;
               case UserAction.ApplySolution:
                  ApplySolution( prOut, out prOut, chosenSolution, context );
                  break;
            }
         }

         return true;
      }

      private static void ApplySolution(
            IParserResult prIn,
            out IParserResult prOut,
            ISolution<IParserResult> chosenSolution,
            IBehaviors context ) {
         prOut = (IParserResult)prIn.Clone( );
         foreach( IAction<IParserResult> action in chosenSolution.Actions )
            action.Apply( prOut, context );
      }

      private static void AutoSolve(
            IParserResult prIn,
            out IParserResult prOut,
            IEnumerable<IRule<IParserResult>> rules,
            IBehaviors context ) {
         prOut = (IParserResult)prIn.Clone( );
         GenericRuleChecker<IParserResult> grc = new GenericRuleChecker<IParserResult>( rules, prOut, true, context );
         prOut = grc.Subject;
      }

      private UserAction Prompt(
            IEnumerable<ISolution<IParserResult>> solutions,
            out ISolution<IParserResult> solution ) {
         solutions = solutions.Union( __defaultSolutions ).ToList( );

         solution = _userPrompt( (IList<ISolution<IParserResult>>)solutions );

         if( solution == __cancelSolution )
            return UserAction.Cancel;
         if( solution == __autoSolveSolution )
            return UserAction.AutoSolve;

         return UserAction.ApplySolution;
      }

      private ISolution<IParserResult> UserPrompt( IList<ISolution<IParserResult>> solutions ) {
         int index = 1;
         foreach( ISolution<IParserResult> option in solutions )
            Console.WriteLine( $"{index++}\t {option.Description}" );

         Console.Write( "enter option: " );
         int count = solutions.Count;
         while( !int.TryParse( Console.ReadLine( ), out index )
               ||index < 1
               ||index > count )
            Console.Write( $"{Environment.NewLine}enter valid option: " );

         return solutions[ index-1 ];
      }

      private enum UserAction {

         ApplySolution,
         AutoSolve,
         Cancel

      }

   }

}