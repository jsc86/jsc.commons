// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.hierarchy.backend.interfaces;
using jsc.commons.hierarchy.path;

namespace jsc.commons.hierarchy.backend.exceptions {

   public class BackendMissingMetaException : BackendExceptionBase {

      public BackendMissingMetaException( IHierarchyBackend backend, Path path, string message ) : base(
            backend,
            message ) {
         Path = path;
      }

      public BackendMissingMetaException(
            IHierarchyBackend backend,
            Path path,
            string message,
            Exception innerException ) : base( backend, message, innerException ) {
         Path = path;
      }

      public Path Path { get; }

   }

}