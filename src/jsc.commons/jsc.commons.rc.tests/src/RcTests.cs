// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Collections.Generic;
using System.Linq;

using jsc.commons.rc.generic.rules;
using jsc.commons.rc.interfaces;
using jsc.commons.rc.listsubject.actions;
using jsc.commons.rc.listsubject.rules;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace jsc.commons.rc.tests {

   [TestFixture]
   public class RcTests {

      private void AssertAndViolation( IViolation<IList<string>> violation, IList<string> subject ) {
         ClassicAssert.IsInstanceOf<Violation<IList<string>>>( violation );
         ClassicAssert.AreEqual( 1, violation.Solutions.Count( ) );
         ClassicAssert.AreEqual( 2-subject.Count, violation.Solutions.First( ).Actions.Count( ) );
         if( subject.Contains( "world" ) ) {
            Add<string> addA =
                  violation.Solutions.First( )
                              .Actions.FirstOrDefault( a => a is Add<string>&&( (Add<string>)a ).Target == "hello" ) as
                        Add<string>;
            ClassicAssert.NotNull( addA );
         }

         if( subject.Contains( "hello" ) ) {
            Add<string> addB =
                  violation.Solutions.First( )
                              .Actions.FirstOrDefault( a => a is Add<string>&&( (Add<string>)a ).Target == "world" ) as
                        Add<string>;
            ClassicAssert.NotNull( addB );
         }
      }

      private void AddN( And<IList<string>> rule, params string[] args ) {
         IRuleChecker<IList<string>> rc = new RuleCheckerBase<IList<string>>( );
         rc.Add( rule );
         IList<string> list = args.Where( arg => arg != null ).ToList( );
         IViolation<IList<string>> violation = rc.Check( list );
         if( args.Length == args.Count( arg => arg != null ) )
            ClassicAssert.IsInstanceOf<NonViolation<IList<string>>>( violation );
         else
            ClassicAssert.IsNotInstanceOf<NonViolation<IList<string>>>( violation );
      }

      private void OrN( Or<IList<string>> rule, params string[] args ) {
         IRuleChecker<IList<string>> rc = new RuleCheckerBase<IList<string>>( );
         rc.Add( rule );
         IList<string> list = args.Where( arg => arg != null ).ToList( );
         IViolation<IList<string>> violation = rc.Check( list );
         if( args.Count( arg => arg != null ) > 0 )
            ClassicAssert.IsInstanceOf<NonViolation<IList<string>>>( violation );
         else
            ClassicAssert.IsNotInstanceOf<NonViolation<IList<string>>>( violation );
      }

      private void XorN( Xor<IList<string>> rule, params string[] args ) {
         IRuleChecker<IList<string>> rc = new RuleCheckerBase<IList<string>>( );
         rc.Add( rule );
         IList<string> list = args.Where( arg => arg != null ).ToList( );
         IViolation<IList<string>> violation = rc.Check( list );
         if( args.Count( arg => arg != null ) == 1 )
            ClassicAssert.IsInstanceOf<NonViolation<IList<string>>>( violation );
         else
            ClassicAssert.IsNotInstanceOf<NonViolation<IList<string>>>( violation );
      }

      [Test]
      [Combinatorial]
      public void Add3(
            [Values( null, "a" )] string a,
            [Values( null, "b" )] string b,
            [Values( null, "c" )] string c ) {
         AddN(
               new And<IList<string>>(
                     new Contains<string>( "a" ),
                     new Contains<string>( "b" ),
                     new Contains<string>( "c" ) ),
               a,
               b,
               c );
      }

      [Test]
      [Combinatorial]
      public void Add4(
            [Values( null, "a" )] string a,
            [Values( null, "b" )] string b,
            [Values( null, "c" )] string c,
            [Values( null, "d" )] string d ) {
         AddN(
               new And<IList<string>>(
                     new Contains<string>( "a" ),
                     new Contains<string>( "b" ),
                     new Contains<string>( "c" ),
                     new Contains<string>( "d" ) ),
               a,
               b,
               c,
               d );
      }

      [Test]
      public void And( ) {
         IRuleChecker<IList<string>> rc = new RuleCheckerBase<IList<string>>( );
         rc.Add( new And<IList<string>>( new Contains<string>( "hello" ), new Contains<string>( "world" ) ) );
         IList<string> list = new List<string>( );

         IViolation<IList<string>> violation = rc.Check( list );
         AssertAndViolation( violation, list );

         list.Add( "hello" );
         violation = rc.Check( list );
         AssertAndViolation( violation, list );

         list.Remove( "hello" );
         list.Add( "world" );
         violation = rc.Check( list );
         AssertAndViolation( violation, list );

         list.Add( "hello" );
         violation = rc.Check( list );
         ClassicAssert.IsInstanceOf<NonViolation<IList<string>>>( violation );
      }

      [Test]
      public void ContainsNoViolation( ) {
         IRuleChecker<IList<string>> rc = new RuleCheckerBase<IList<string>>( );
         rc.Add( new Contains<string>( "hello" ) );
         IList<string> list = new List<string> {
               "hello"
         };
         ClassicAssert.AreEqual( rc.Check( list ), NonViolation<IList<string>>.Instance );
      }

      [Test]
      public void ContainsViolation( ) {
         IRuleChecker<IList<string>> rc = new RuleCheckerBase<IList<string>>( );
         rc.Add( new Contains<string>( "hello" ) );
         IList<string> list = new List<string>( );
         IViolation<IList<string>> violation = rc.Check( list );

         ClassicAssert.IsInstanceOf<Violation<IList<string>>>( violation );
         ClassicAssert.AreEqual( 1, violation.Solutions.Count( ) );
         ClassicAssert.AreEqual( 1, violation.Solutions.First( ).Actions.Count( ) );
         ClassicAssert.IsInstanceOf<Add<string>>( violation.Solutions.First( ).Actions.First( ) );
         ClassicAssert.AreEqual( "hello", ( violation.Solutions.First( ).Actions.First( ) as Add<string> )?.Target );
      }

      [Test]
      public void ImpliesNoViolation( ) {
         IRuleChecker<IList<string>> rc = new RuleCheckerBase<IList<string>>( );
         rc.Add(
               new Implies<IList<string>>(
                     new Contains<string>( "hello" ),
                     new Contains<string>( "world" ) ) );
         IList<string> list = new List<string>( );
         IViolation<IList<string>> violation = rc.Check( list );
         ClassicAssert.IsInstanceOf<NonViolation<IList<string>>>( violation );

         list.Add( "world" );
         violation = rc.Check( list );
         ClassicAssert.IsInstanceOf<NonViolation<IList<string>>>( violation );

         list.Add( "hello" );
         violation = rc.Check( list );
         ClassicAssert.IsInstanceOf<NonViolation<IList<string>>>( violation );
      }

      [Test]
      public void ImpliesViolation( ) {
         IRuleChecker<IList<string>> rc = new RuleCheckerBase<IList<string>>( );
         rc.Add(
               new Implies<IList<string>>(
                     new Contains<string>( "hello" ),
                     new Contains<string>( "world" ) ) );
         IList<string> list = new List<string> {
               "hello"
         };
         IViolation<IList<string>> violation = rc.Check( list );

         ClassicAssert.IsInstanceOf<Violation<IList<string>>>( violation );
         ClassicAssert.AreEqual( 2, violation.Solutions.Count( ) );

         ISolution<IList<string>> solRemA =
               violation.Solutions.FirstOrDefault( s => s.Actions.FirstOrDefault( ) is Remove<string> );
         ClassicAssert.NotNull( solRemA );
         ClassicAssert.AreEqual( 1, solRemA.Actions.Count( ) );
         ClassicAssert.AreEqual( "hello", ( solRemA.Actions.FirstOrDefault( ) as Remove<string> )?.Target );

         ISolution<IList<string>> solAddB =
               violation.Solutions.FirstOrDefault( s => s.Actions.FirstOrDefault( ) is Add<string> );
         ClassicAssert.NotNull( solAddB );
         ClassicAssert.AreEqual( 1, solAddB.Actions.Count( ) );
         ClassicAssert.AreEqual( "world", ( solAddB.Actions.FirstOrDefault( ) as Add<string> )?.Target );

         ClassicAssert.AreEqual( solAddB, violation.Solutions.FirstOrDefault( ) );
      }

      [Test]
      public void Or( ) {
         IRuleChecker<IList<string>> rc = new RuleCheckerBase<IList<string>>( );
         rc.Add( new Or<IList<string>>( new Contains<string>( "hello" ), new Contains<string>( "world" ) ) );
         IList<string> list = new List<string>( );

         IViolation<IList<string>> violation = rc.Check( list );
         ClassicAssert.IsInstanceOf<Violation<IList<string>>>( violation );
         ClassicAssert.AreEqual( 2, violation.Solutions.Count( ) );
         ISolution<IList<string>> solAddA = violation.Solutions
               .FirstOrDefault(
                     s => s.Actions.FirstOrDefault( ) is Add<string>&&
                           ( s.Actions.FirstOrDefault( ) as Add<string> )?.Target == "hello" );
         ClassicAssert.IsNotNull( solAddA );
         ClassicAssert.AreEqual( 1, solAddA.Actions.Count( ) );
         ISolution<IList<string>> solAddB = violation.Solutions
               .FirstOrDefault(
                     s => s.Actions.FirstOrDefault( ) is Add<string>&&
                           ( s.Actions.FirstOrDefault( ) as Add<string> )?.Target == "world" );
         ClassicAssert.IsNotNull( solAddB );
         ClassicAssert.AreEqual( 1, solAddB.Actions.Count( ) );

         list.Add( "hello" );
         violation = rc.Check( list );
         ClassicAssert.IsInstanceOf<NonViolation<IList<string>>>( violation );

         list.Remove( "hello" );
         list.Add( "world" );
         violation = rc.Check( list );
         ClassicAssert.IsInstanceOf<NonViolation<IList<string>>>( violation );

         list.Add( "hello" );
         violation = rc.Check( list );
         ClassicAssert.IsInstanceOf<NonViolation<IList<string>>>( violation );
      }

      [Test]
      [Combinatorial]
      public void Or3(
            [Values( null, "a" )] string a,
            [Values( null, "b" )] string b,
            [Values( null, "c" )] string c ) {
         OrN(
               new Or<IList<string>>(
                     new Contains<string>( "a" ),
                     new Contains<string>( "b" ),
                     new Contains<string>( "c" ) ),
               a,
               b,
               c );
      }

      [Test]
      [Combinatorial]
      public void Or4(
            [Values( null, "a" )] string a,
            [Values( null, "b" )] string b,
            [Values( null, "c" )] string c,
            [Values( null, "c" )] string d ) {
         OrN(
               new Or<IList<string>>(
                     new Contains<string>( "a" ),
                     new Contains<string>( "b" ),
                     new Contains<string>( "c" ),
                     new Contains<string>( "d" ) ),
               a,
               b,
               c,
               d );
      }

      [Test]
      [Combinatorial]
      public void Xor3(
            [Values( null, "a" )] string a,
            [Values( null, "b" )] string b,
            [Values( null, "c" )] string c ) {
         XorN(
               new Xor<IList<string>>(
                     new Contains<string>( "a" ),
                     new Contains<string>( "b" ),
                     new Contains<string>( "c" ) ),
               a,
               b,
               c );
      }

      [Test]
      [Combinatorial]
      public void Xor4(
            [Values( null, "a" )] string a,
            [Values( null, "b" )] string b,
            [Values( null, "c" )] string c,
            [Values( null, "d" )] string d ) {
         XorN(
               new Xor<IList<string>>(
                     new Contains<string>( "a" ),
                     new Contains<string>( "b" ),
                     new Contains<string>( "c" ),
                     new Contains<string>( "d" ) ),
               a,
               b,
               c,
               d );
      }

      [Test]
      public void XorSolutions( ) {
         IRuleChecker<IList<string>> rc = new RuleCheckerBase<IList<string>>( );
         rc.Add(
               new Xor<IList<string>>(
                     new Contains<string>( "a" ),
                     new Contains<string>( "b" ) ) );

         IList<string> list = new CList<string>( );
         IViolation<IList<string>> violation = rc.Check( list );

         ClassicAssert.IsNotInstanceOf<NonViolation<IList<string>>>( violation );

         ISolution<IList<string>>[] solutions = violation.Solutions.ToArray( );

         ClassicAssert.AreEqual( 2, solutions.Length );

         IAction<IList<string>>[] actions1 = solutions[ 0 ].Actions.ToArray( );
         IAction<IList<string>>[] actions2 = solutions[ 1 ].Actions.ToArray( );

         ClassicAssert.AreEqual( 1, actions1.Length );
         ClassicAssert.AreEqual( 1, actions2.Length );
         ClassicAssert.AreEqual( 1, actions1.Count( a => a is Add<string> ) );
         ClassicAssert.AreEqual( 0, actions1.Count( a => a is Remove<string> ) );
         ClassicAssert.AreEqual( 1, actions2.Count( a => a is Add<string> ) );
         ClassicAssert.AreEqual( 0, actions2.Count( a => a is Remove<string> ) );

         IAction<IList<string>>[] allActions = actions1.Union( actions2 ).ToArray( );

         ClassicAssert.AreEqual( 1, allActions.Count( a => a is Add<string> add&&add.Target == "a" ) );
         ClassicAssert.AreEqual( 1, allActions.Count( a => a is Add<string> add&&add.Target == "b" ) );
         ClassicAssert.AreEqual( 0, allActions.Count( a => a is Remove<string> rem&&rem.Target == "a" ) );
         ClassicAssert.AreEqual( 0, allActions.Count( a => a is Remove<string> rem&&rem.Target == "b" ) );
      }

   }

}
