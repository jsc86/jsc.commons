// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Runtime.Serialization;

namespace jsc.commons.hierarchy.exceptions {

   public class HierarchyManagerException : Exception {

      public HierarchyManagerException( ) { }

      protected HierarchyManagerException( SerializationInfo info, StreamingContext context ) :
            base( info, context ) { }

      public HierarchyManagerException( string message ) : base( message ) { }
      public HierarchyManagerException( string message, Exception innerException ) : base( message, innerException ) { }

   }

}
