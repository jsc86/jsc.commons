// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System.Linq;

using jsc.commons.behaving.interfaces;
using jsc.commons.cli.interfaces;
using jsc.commons.rc;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.policies {

   public class UniqueNames : RuleBase<ICliSpecification> {

      public override IViolation<ICliSpecification> Check(
            ICliSpecification subject,
            IBehaviors context = null ) {
         IOption opt1 =
               subject.Options.FirstOrDefault( o1 => subject.Options.Any( o2 => o1 != o2&&o1.Name.Equals( o2.Name ) ) );
         if( opt1 != null ) {
            IOption opt2 = subject.Options.First( o2 => o2 != opt1&&o2.Name.Equals( opt1.Name ) );
            return new Violation<ICliSpecification>(
                  this,
                  Enumerable.Empty<ISolution<ICliSpecification>>( ),
                  $"name collision between options {opt1.Name} and {opt2.Name}" );
         }

         IFlag flag1 =
               subject.Flags.FirstOrDefault( f1 => subject.Flags.Any( f2 => f1 != f2&&f1.Name == f2.Name ) );
         if( flag1 != null ) {
            IFlag flag2 = subject.Flags.First( f2 => f2 != flag1&&f2.Name == flag1.Name );
            return new Violation<ICliSpecification>(
                  this,
                  Enumerable.Empty<ISolution<ICliSpecification>>( ),
                  $"name collision between flags {flag1.Name} and {flag2.Name}" );
         }

         opt1 = subject.Options.FirstOrDefault(
               o1 => o1.FlagAlias.HasValue&&subject.Flags.Any( f1 => o1.FlagAlias.Value == f1.Name ) );
         if( opt1 != null ) {
            // ReSharper disable once PossibleInvalidOperationException
            flag1 = subject.Flags.First( f1 => f1.Name == opt1.FlagAlias.Value );
            return new Violation<ICliSpecification>(
                  this,
                  Enumerable.Empty<ISolution<ICliSpecification>>( ),
                  $"name collision between option {opt1.Name} flag alias and flag {flag1.Name}" );
         }

         opt1 = subject.Options.FirstOrDefault(
               o1 => subject.Options.Any(
                     o2 => o1 != o2&&
                           o1.FlagAlias.HasValue&&
                           o2.FlagAlias.HasValue&&
                           o1.FlagAlias.Value == o2.FlagAlias.Value ) );
         if( opt1 != null ) {
            IOption opt2 = subject.Options.First(
                  // ReSharper disable once PossibleInvalidOperationException
                  o2 => o2 != opt1&&o2.FlagAlias.HasValue&&o2.FlagAlias.Value == opt1.FlagAlias.Value );
            return new Violation<ICliSpecification>(
                  this,
                  Enumerable.Empty<ISolution<ICliSpecification>>( ),
                  $"flag alias name collision between options {opt1.Name} and {opt2.Name}" );
         }

         return NonViolation<ICliSpecification>.Instance;
      }

   }

}