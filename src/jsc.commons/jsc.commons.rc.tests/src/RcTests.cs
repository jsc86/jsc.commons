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

namespace jsc.commons.rc.tests {

   [TestFixture]
   public class RcTests {

      private void AssertAndViolation( IViolation<IList<string>> violation ) {
         Assert.IsInstanceOf<Violation<IList<string>>>( violation );
         Assert.AreEqual( 1, violation.Solutions.Count( ) );
         Assert.AreEqual( 2, violation.Solutions.First( ).Actions.Count( ) );
         Add<string> addA =
               violation.Solutions.First( )
                           .Actions.FirstOrDefault( a => a is Add<string>&&( (Add<string>)a ).Target == "hello" ) as
                     Add<string>;
         Assert.NotNull( addA );
         Add<string> addB =
               violation.Solutions.First( )
                           .Actions.FirstOrDefault( a => a is Add<string>&&( (Add<string>)a ).Target == "world" ) as
                     Add<string>;
         Assert.NotNull( addB );
      }

      private void AddN( And<IList<string>> rule, params string[] args ) {
         IRuleChecker<IList<string>> rc = new RuleCheckerBase<IList<string>>( );
         rc.Add( rule );
         IList<string> list = args.Where( arg => arg != null ).ToList( );
         IViolation<IList<string>> violation = rc.Check( list );
         if( args.Length == args.Count( arg => arg != null ) )
            Assert.IsInstanceOf<NonViolation<IList<string>>>( violation );
         else
            Assert.IsNotInstanceOf<NonViolation<IList<string>>>( violation );
      }

      private void OrN( Or<IList<string>> rule, params string[] args ) {
         IRuleChecker<IList<string>> rc = new RuleCheckerBase<IList<string>>( );
         rc.Add( rule );
         IList<string> list = args.Where( arg => arg != null ).ToList( );
         IViolation<IList<string>> violation = rc.Check( list );
         if( args.Count( arg => arg != null ) > 0 )
            Assert.IsInstanceOf<NonViolation<IList<string>>>( violation );
         else
            Assert.IsNotInstanceOf<NonViolation<IList<string>>>( violation );
      }

      private void XorN( Xor<IList<string>> rule, params string[] args ) {
         IRuleChecker<IList<string>> rc = new RuleCheckerBase<IList<string>>( );
         rc.Add( rule );
         IList<string> list = args.Where( arg => arg != null ).ToList( );
         IViolation<IList<string>> violation = rc.Check( list );
         if( args.Count( arg => arg != null ) == 1 )
            Assert.IsInstanceOf<NonViolation<IList<string>>>( violation );
         else
            Assert.IsNotInstanceOf<NonViolation<IList<string>>>( violation );
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
         AssertAndViolation( violation );

         list.Add( "hello" );
         violation = rc.Check( list );
         AssertAndViolation( violation );

         list.Remove( "hello" );
         list.Add( "world" );
         violation = rc.Check( list );
         AssertAndViolation( violation );

         list.Add( "hello" );
         violation = rc.Check( list );
         Assert.IsInstanceOf<NonViolation<IList<string>>>( violation );
      }

      [Test]
      public void ContainsNoViolation( ) {
         IRuleChecker<IList<string>> rc = new RuleCheckerBase<IList<string>>( );
         rc.Add( new Contains<string>( "hello" ) );
         IList<string> list = new List<string> {
               "hello"
         };
         Assert.AreEqual( rc.Check( list ), NonViolation<IList<string>>.Instance );
      }

      [Test]
      public void ContainsViolation( ) {
         IRuleChecker<IList<string>> rc = new RuleCheckerBase<IList<string>>( );
         rc.Add( new Contains<string>( "hello" ) );
         IList<string> list = new List<string>( );
         IViolation<IList<string>> violation = rc.Check( list );

         Assert.IsInstanceOf<Violation<IList<string>>>( violation );
         Assert.AreEqual( 1, violation.Solutions.Count( ) );
         Assert.AreEqual( 1, violation.Solutions.First( ).Actions.Count( ) );
         Assert.IsInstanceOf<Add<string>>( violation.Solutions.First( ).Actions.First( ) );
         Assert.AreEqual( "hello", ( violation.Solutions.First( ).Actions.First( ) as Add<string> )?.Target );
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
         Assert.IsInstanceOf<NonViolation<IList<string>>>( violation );

         list.Add( "world" );
         violation = rc.Check( list );
         Assert.IsInstanceOf<NonViolation<IList<string>>>( violation );

         list.Add( "hello" );
         violation = rc.Check( list );
         Assert.IsInstanceOf<NonViolation<IList<string>>>( violation );
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

         Assert.IsInstanceOf<Violation<IList<string>>>( violation );
         Assert.AreEqual( 2, violation.Solutions.Count( ) );

         ISolution<IList<string>> solRemA =
               violation.Solutions.FirstOrDefault( s => s.Actions.FirstOrDefault( ) is Remove<string> );
         Assert.NotNull( solRemA );
         Assert.AreEqual( 1, solRemA.Actions.Count( ) );
         Assert.AreEqual( "hello", ( solRemA.Actions.FirstOrDefault( ) as Remove<string> )?.Target );

         ISolution<IList<string>> solAddB =
               violation.Solutions.FirstOrDefault( s => s.Actions.FirstOrDefault( ) is Add<string> );
         Assert.NotNull( solAddB );
         Assert.AreEqual( 1, solAddB.Actions.Count( ) );
         Assert.AreEqual( "world", ( solAddB.Actions.FirstOrDefault( ) as Add<string> )?.Target );

         Assert.AreEqual( solAddB, violation.Solutions.FirstOrDefault( ) );
      }

      [Test]
      public void Or( ) {
         IRuleChecker<IList<string>> rc = new RuleCheckerBase<IList<string>>( );
         rc.Add( new Or<IList<string>>( new Contains<string>( "hello" ), new Contains<string>( "world" ) ) );
         IList<string> list = new List<string>( );

         IViolation<IList<string>> violation = rc.Check( list );
         Assert.IsInstanceOf<Violation<IList<string>>>( violation );
         Assert.AreEqual( 2, violation.Solutions.Count( ) );
         ISolution<IList<string>> solAddA = violation.Solutions
               .FirstOrDefault(
                     s => s.Actions.FirstOrDefault( ) is Add<string>&&
                           ( s.Actions.FirstOrDefault( ) as Add<string> )?.Target == "hello" );
         Assert.IsNotNull( solAddA );
         Assert.AreEqual( 1, solAddA.Actions.Count( ) );
         ISolution<IList<string>> solAddB = violation.Solutions
               .FirstOrDefault(
                     s => s.Actions.FirstOrDefault( ) is Add<string>&&
                           ( s.Actions.FirstOrDefault( ) as Add<string> )?.Target == "world" );
         Assert.IsNotNull( solAddB );
         Assert.AreEqual( 1, solAddB.Actions.Count( ) );

         list.Add( "hello" );
         violation = rc.Check( list );
         Assert.IsInstanceOf<NonViolation<IList<string>>>( violation );

         list.Remove( "hello" );
         list.Add( "world" );
         violation = rc.Check( list );
         Assert.IsInstanceOf<NonViolation<IList<string>>>( violation );

         list.Add( "hello" );
         violation = rc.Check( list );
         Assert.IsInstanceOf<NonViolation<IList<string>>>( violation );
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

   }

}
