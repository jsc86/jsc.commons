// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.hierarchy.backend.interfaces;

namespace jsc.commons.hierarchy.backend.exceptions {

   public class BackendInitializationException : BackendExceptionBase {

      public BackendInitializationException( IHierarchyBackend backend, string message ) : base( backend, message ) { }

      public BackendInitializationException( IHierarchyBackend backend, string message, Exception innerException ) :
            base( backend, message, innerException ) { }

   }

}