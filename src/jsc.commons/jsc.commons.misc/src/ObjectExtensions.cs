using System;

namespace jsc.commons.misc {

   public static class ObjectExtensions {

      public static void MustNotBeNull( this object obj, string argument ) {
         if( obj == null )
            throw new ArgumentNullException( argument, $"{argument} must not be null" );
      }

   }

}
