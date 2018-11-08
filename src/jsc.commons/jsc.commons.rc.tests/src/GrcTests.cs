// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

using jsc.commons.rc.generic.rules;
using jsc.commons.rc.interfaces;
using jsc.commons.rc.listsubject.actions;
using jsc.commons.rc.listsubject.rules;

using NUnit.Framework;

namespace jsc.commons.rc.tests {

   [TestFixture]
   public class GrcTests {

      [Test]
      public void GrcApplyNoViolation( ) {
         GenericRuleChecker<IList<string>> grc = new GenericRuleChecker<IList<string>>( subject: new CList<string>( ) );
         Assert.AreEqual( 0, grc.Subject.Count );
         grc.Apply( new Add<string>( "hello" ) ).Accept( );
         Assert.AreEqual( 1, grc.Subject.Count );
         Assert.AreEqual( "hello", grc.Subject[ 0 ] );
      }

      [Test]
      public void GrcApplyViolation( ) {
         GenericRuleChecker<IList<string>> grc = new GenericRuleChecker<IList<string>>(
               new[] {
                     new Implies<IList<string>>( new Contains<string>( "hello" ), new Contains<string>( "world" ) )
               },
               new CList<string>( ) );
         Assert.AreEqual( 0, grc.Subject.Count );
         ApplicationResult<IList<string>> ar = grc.Apply( new Add<string>( "hello" ) );
         Assert.IsNotNull( ar );
         Assert.IsTrue( ar.Successful );
         ar.Accept( );
         Assert.AreEqual( 2, grc.Subject.Count );
         Assert.IsTrue( grc.Subject.Contains( "hello" ) );
         Assert.IsTrue( grc.Subject.Contains( "world" ) );
      }

      [Test]
      public void GrcCtorTest1( ) {
         Exception exc = null;
         try {
            IList<string> list = new CList<string>( );
            IList<IRule<IList<string>>> rules = new List<IRule<IList<string>>>( );
            GenericRuleChecker<IList<string>> grc = new GenericRuleChecker<IList<string>>( rules, list );
            Assert.AreEqual( 0, grc.Subject.Count );
         } catch( Exception e ) {
            exc = e;
         }

         Assert.IsNull( exc );
      }

      [Test]
      public void GrcCtorTest2( ) {
         Exception exc = null;
         try {
            IList<string> list = new CList<string>( );
            IList<IRule<IList<string>>> rules = new List<IRule<IList<string>>> {
                  new Contains<string>( "hello" )
            };
            GenericRuleChecker<IList<string>> grc = new GenericRuleChecker<IList<string>>( rules, list );
         } catch( Exception e ) {
            exc = e;
         }

         Assert.IsNotNull( exc );
      }

      [Test]
      public void GrcCtorTest3( ) {
         Exception exc = null;
         try {
            IList<string> list = new CList<string>( );
            IList<IRule<IList<string>>> rules = new List<IRule<IList<string>>> {
                  new Contains<string>( "hello" )
            };
            GenericRuleChecker<IList<string>> grc =
                  new GenericRuleChecker<IList<string>>(
                        rules,
                        list,
                        true );
            Assert.AreEqual( 1, grc.Subject.Count );
            Assert.AreEqual( "hello", grc.Subject[ 0 ] );
         } catch( Exception e ) {
            exc = e;
         }

         Assert.IsNull( exc );
      }

   }

}