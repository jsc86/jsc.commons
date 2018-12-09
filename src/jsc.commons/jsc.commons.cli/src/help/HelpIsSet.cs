using System.Collections.Generic;
using System.Linq;

using jsc.commons.behaving.interfaces;
using jsc.commons.cli.interfaces;
using jsc.commons.rc;
using jsc.commons.rc.interfaces;

namespace jsc.commons.cli.help {

   internal class HelpIsSet : RuleBase<IParserResult> {

      private readonly HelpOption _target;

      private string _description;

      public HelpIsSet( HelpOption target ) {
         _target = target;
      }

      public IItem Target => _target;

      public override string Description =>
            _description??
            ( _description = $"option {_target.Name} is set" );

      public override IViolation<IParserResult> Check( IParserResult subject, IBehaviors context = null ) {
         if( subject.IsSet( Target ) )
            return NonViolation<IParserResult>.Instance;

         return new Violation<IParserResult>(
               this,
               MakeValid( )
         );
      }

      public override IEnumerable<ISolution<IParserResult>> MakeValid( ) {
         return Enumerable.Empty<ISolution<IParserResult>>( );
      }

      public override IEnumerable<ISolution<IParserResult>> MakeInvalid( ) {
         return Enumerable.Empty<ISolution<IParserResult>>( );
      }

   }

}
