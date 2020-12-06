// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;

using jsc.commons.hierarchy.backend.interfaces;
using jsc.commons.hierarchy.resources.interfaces;

namespace jsc.commons.hierarchy.backend.exceptions {

   public class BackendWriteException : BackendExceptionBase {

      public BackendWriteException( IHierarchyBackend backend, IResource resource, string message ) : base(
            backend,
            message ) {
         Resource = resource;
      }

      public BackendWriteException(
            IHierarchyBackend backend,
            IResource resource,
            string message,
            Exception innerException ) : base(
            backend,
            message,
            innerException ) {
         Resource = resource;
      }

      public IResource Resource { get; }

   }

}