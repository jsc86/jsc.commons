// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Linq;

using jsc.commons.behaving.interfaces;
using jsc.commons.cli.config;
using jsc.commons.cli.interfaces;
using jsc.commons.rc;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.policies {

   public class UniqueNames : RuleBase<ICliSpecification> {

      private static readonly Func<UniqueNames, ICliSpecification, ICliConfig, IViolation<ICliSpecification>>[]
            __checks;

      static UniqueNames( ) {
         __checks = new Func<UniqueNames, ICliSpecification, ICliConfig, IViolation<ICliSpecification>>[] {
               CheckOptionNames,
               CheckOptionAliases,
               CheckFlagNames,
               CheckFlagNamesAndOptionAliases
         };
      }

      public override IViolation<ICliSpecification> Check(
            ICliSpecification subject,
            IBehaviors context = null ) {
         ConfigBehavior cb = null;
         context?.TryGet( out cb );
         ICliConfig conf = cb?.Config;

         foreach( Func<UniqueNames, ICliSpecification, ICliConfig, IViolation<ICliSpecification>> check
               in __checks ) {
            IViolation<ICliSpecification> violation = check( this, subject, conf );
            if( violation != NonViolation<ICliSpecification>.Instance )
               return violation;
         }

         return NonViolation<ICliSpecification>.Instance;
      }

      private static IViolation<ICliSpecification> CheckOptionNames(
            UniqueNames _this,
            ICliSpecification subject,
            ICliConfig conf ) {
         bool CmpOptNames( IOption o1, IOption o2 ) {
            return o1 != o2&&o1.Name.Equals(
                  o2.Name,
                  conf?.CaseSensitiveOptions??true
                        ? StringComparison.Ordinal
                        : StringComparison.OrdinalIgnoreCase );
         }

         IOption opt1 = subject.Options.FirstOrDefault( o1 => subject.Options.Any( o2 => CmpOptNames( o1, o2 ) ) );

         if( opt1 != null ) {
            IOption opt2 = subject.Options.First( o2 => CmpOptNames( opt1, o2 ) );
            return new Violation<ICliSpecification>(
                  _this,
                  Enumerable.Empty<ISolution<ICliSpecification>>( ),
                  $"name collision between options {opt1.Name} and {opt2.Name}" );
         }

         return NonViolation<ICliSpecification>.Instance;
      }

      private static IViolation<ICliSpecification> CheckFlagNames(
            UniqueNames _this,
            ICliSpecification subject,
            ICliConfig conf ) {
         bool CmpFlagNames( IFlag f1, IFlag f2 ) {
            return f1 != f2&&CmpChars( f1.Name, f2.Name, conf?.CaseSensitiveFlags??true );
         }

         IFlag flag1 =
               subject.Flags.FirstOrDefault( f1 => subject.Flags.Any( f2 => CmpFlagNames( f1, f2 ) ) );
         if( flag1 != null ) {
            IFlag flag2 = subject.Flags.First( f2 => CmpFlagNames( flag1, f2 ) );
            return new Violation<ICliSpecification>(
                  _this,
                  Enumerable.Empty<ISolution<ICliSpecification>>( ),
                  $"name collision between flags {flag1.Name} and {flag2.Name}" );
         }

         return NonViolation<ICliSpecification>.Instance;
      }

      private static IViolation<ICliSpecification> CheckFlagNamesAndOptionAliases(
            UniqueNames _this,
            ICliSpecification subject,
            ICliConfig conf ) {
         bool CmpOptAliasFlag( IOption opt, IFlag flag ) {
            return CmpChars(
                  // ReSharper disable once PossibleInvalidOperationException
                  opt.FlagAlias.Value,
                  flag.Name,
                  conf?.CaseSensitiveFlags??true );
         }

         IOption opt1 = subject.Options.FirstOrDefault(
               o1 => o1.FlagAlias.HasValue&&subject.Flags.Any( f1 => CmpOptAliasFlag( o1, f1 ) ) );

         if( opt1 != null ) {
            IFlag flag1 = subject.Flags.First( f1 => CmpOptAliasFlag( opt1, f1 ) );
            return new Violation<ICliSpecification>(
                  _this,
                  Enumerable.Empty<ISolution<ICliSpecification>>( ),
                  $"name collision between option {opt1.Name} flag alias and flag {flag1.Name}" );
         }

         return NonViolation<ICliSpecification>.Instance;
      }

      private static IViolation<ICliSpecification> CheckOptionAliases(
            UniqueNames _this,
            ICliSpecification subject,
            ICliConfig conf ) {
         bool CmpOptAliases( IOption o1, IOption o2 ) {
            return o1 != o2&&
                  o1.FlagAlias.HasValue&&
                  o2.FlagAlias.HasValue&&
                  CmpChars( o1.FlagAlias.Value, o2.FlagAlias.Value, conf?.CaseSensitiveFlags??true );
         }

         IOption opt1 = subject.Options.FirstOrDefault( o1 => subject.Options.Any( o2 => CmpOptAliases( o1, o2 ) ) );
         if( opt1 != null ) {
            IOption opt2 = subject.Options.First(
                  o2 => CmpOptAliases( opt1, o2 ) );
            return new Violation<ICliSpecification>(
                  _this,
                  Enumerable.Empty<ISolution<ICliSpecification>>( ),
                  $"flag alias name collision between options {opt1.Name} and {opt2.Name}" );
         }

         return NonViolation<ICliSpecification>.Instance;
      }


      private static bool CmpChars( char c1, char c2, bool cs ) {
         return cs? c1 == c2 : char.ToUpperInvariant( c1 ) == char.ToUpperInvariant( c2 );
      }

   }

}