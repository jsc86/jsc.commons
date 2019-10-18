using System;

namespace jsc.commons.unidto.core.attributes {

   [AttributeUsage( AttributeTargets.Interface )]
   public class ImplementationAttribute : Attribute {

      public ImplementationAttribute( Type type ) {
         Type = type;
      }

      public Type Type { get; set; }

   }

}