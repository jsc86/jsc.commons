// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.rc.generic.rules;
using jsc.commons.rc.interfaces;

using NUnit.Framework;

namespace jsc.commons.rc.tests {

   [TestFixture]
   public class GenericRuleTests {

      [Test]
      public void AndFalseFalse( ) {
         And<object> and = new And<object>( False<object>.Instance, False<object>.Instance );
         IViolation<object> violation = and.Check( null );
         Assert.AreNotEqual( NonViolation<object>.Instance, violation );
      }

      [Test]
      public void AndFalseTrue( ) {
         And<object> and = new And<object>( False<object>.Instance, True<object>.Instance );
         IViolation<object> violation = and.Check( null );
         Assert.AreNotEqual( NonViolation<object>.Instance, violation );
      }

      [Test]
      public void AndTrueFalse( ) {
         And<object> and = new And<object>( True<object>.Instance, False<object>.Instance );
         IViolation<object> violation = and.Check( null );
         Assert.AreNotEqual( NonViolation<object>.Instance, violation );
      }

      [Test]
      public void AndTrueTrue( ) {
         And<object> and = new And<object>( True<object>.Instance, True<object>.Instance );
         IViolation<object> violation = and.Check( null );
         Assert.AreEqual( NonViolation<object>.Instance, violation );
      }

      [Test]
      public void ImpliesFalseFalse( ) {
         Implies<object> implies = new Implies<object>( False<object>.Instance, False<object>.Instance );
         IViolation<object> violation = implies.Check( null );
         Assert.AreEqual( NonViolation<object>.Instance, violation );
      }

      [Test]
      public void ImpliesFalseTrue( ) {
         Implies<object> implies = new Implies<object>( False<object>.Instance, True<object>.Instance );
         IViolation<object> violation = implies.Check( null );
         Assert.AreEqual( NonViolation<object>.Instance, violation );
      }

      [Test]
      public void ImpliesTrueFalse( ) {
         Implies<object> implies = new Implies<object>( True<object>.Instance, False<object>.Instance );
         IViolation<object> violation = implies.Check( null );
         Assert.AreNotEqual( NonViolation<object>.Instance, violation );
      }

      [Test]
      public void ImpliesTrueTrue( ) {
         Implies<object> implies = new Implies<object>( True<object>.Instance, True<object>.Instance );
         IViolation<object> violation = implies.Check( null );
         Assert.AreEqual( NonViolation<object>.Instance, violation );
      }

      [Test]
      public void NotFalse( ) {
         Not<object> not = new Not<object>( False<object>.Instance );
         IViolation<object> violation = not.Check( null );
         Assert.AreEqual( NonViolation<object>.Instance, violation );
      }

      [Test]
      public void NotTrue( ) {
         Not<object> not = new Not<object>( True<object>.Instance );
         IViolation<object> violation = not.Check( null );
         Assert.AreNotEqual( NonViolation<object>.Instance, violation );
      }

      [Test]
      public void OrFalseFalse( ) {
         Or<object> or = new Or<object>( False<object>.Instance, False<object>.Instance );
         IViolation<object> violation = or.Check( null );
         Assert.AreNotEqual( NonViolation<object>.Instance, violation );
      }

      [Test]
      public void OrFalseTrue( ) {
         Or<object> or = new Or<object>( False<object>.Instance, True<object>.Instance );
         IViolation<object> violation = or.Check( null );
         Assert.AreEqual( NonViolation<object>.Instance, violation );
      }

      [Test]
      public void OrTrueFalse( ) {
         Or<object> or = new Or<object>( True<object>.Instance, False<object>.Instance );
         IViolation<object> violation = or.Check( null );
         Assert.AreEqual( NonViolation<object>.Instance, violation );
      }

      [Test]
      public void OrTrueTrue( ) {
         Or<object> or = new Or<object>( True<object>.Instance, True<object>.Instance );
         IViolation<object> violation = or.Check( null );
         Assert.AreEqual( NonViolation<object>.Instance, violation );
      }

   }

}