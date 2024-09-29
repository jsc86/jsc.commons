using System;

namespace jsc.commons.cli.ispec {

   public static class TypeExtensions {

      public static bool IsNullableType( this Type type ) =>
            Nullable.GetUnderlyingType( type ) != null;

   }

}
