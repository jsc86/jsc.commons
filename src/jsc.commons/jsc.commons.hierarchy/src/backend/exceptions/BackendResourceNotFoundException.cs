// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.hierarchy.backend.interfaces;
using jsc.commons.hierarchy.path.interfaces;

namespace jsc.commons.hierarchy.backend.exceptions {

   public class BackendResourceNotFoundException : BackendExceptionBase {

      public BackendResourceNotFoundException( IHierarchyBackend backend, IPath resourcePath, string message ) :
            base( backend, message ) {
         ResourcePath = resourcePath;
      }

      public BackendResourceNotFoundException(
            IHierarchyBackend backend,
            IPath resourcePath,
            string message,
            Exception innerException ) :
            base( backend, message, innerException ) {
         ResourcePath = resourcePath;
      }

      private IPath ResourcePath { get; }

   }

}