// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.naming.styles;

using NUnit.Framework;

namespace jsc.commons.naming.tests {

   [TestFixture]
   public class NamingTests {

      [SetUp]
      public void Init( ) {
         _un = new UnifiedName(
               "short",
               "test",
               "name",
               "FTW" );
      }

      private UnifiedName _un;

      [Test]
      public void CamelCaseFromString( ) {
         Assert.AreEqual( NamingStyles.camelCase.FromString( "shortTestNameFTW" ).ToString( ), "short.test.name.FTW" );
      }

      [Test]
      public void CamelCaseToString( ) {
         Assert.AreEqual( NamingStyles.camelCase.ToString( _un ), "shortTestNameFTW" );
      }

      [Test]
      public void PascalCaseFromString( ) {
         Assert.AreEqual( NamingStyles.PascalCase.FromString( "ShortTestNameFTW" ).ToString( ), "short.test.name.FTW" );
      }

      [Test]
      public void PascalCaseToString( ) {
         Assert.AreEqual( NamingStyles.PascalCase.ToString( _un ), "ShortTestNameFTW" );
      }

      [Test]
      public void SnakeCaseToString( ) {
         Assert.AreEqual( NamingStyles.snake_case.ToString( _un ), "short_test_name_ftw" );
      }

      [Test]
      public void SnakeCaseUpperToString( ) {
         Assert.AreEqual( NamingStyles.SNAKE_CASE.ToString( _un ), "SHORT_TEST_NAME_FTW" );
      }

   }

}