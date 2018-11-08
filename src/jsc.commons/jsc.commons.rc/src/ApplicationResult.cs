// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using jsc.commons.rc.interfaces;

namespace jsc.commons.rc {

   public class ApplicationResult<T> where T : class { //, ICloneable{

      public ApplicationResult(
            T resultSubject,
            IViolation<T> unsolveableViolation,
            ReadOnlyCollection<IAction<T>> performedActions,
            Action acceptDelegate ) {
         ResultSubject = (T)( resultSubject as ICloneable )?.Clone( );
         UnsolvableViolation = unsolveableViolation;
         Successful = ResultSubject != null;
         PerformedActions = performedActions;
         AcceptDelegate = acceptDelegate;
      }

      public T ResultSubject { get; }

      public IViolation<T> UnsolvableViolation { get; }

      public IEnumerable<IAction<T>> PerformedActions { get; }

      public bool Successful { get; }

      private Action AcceptDelegate { get; }

      public void Accept( ) {
         if( !Successful )
            throw new ApplicationException( "cannot accept an invalid configuration" );

         AcceptDelegate( );
      }

   }

}