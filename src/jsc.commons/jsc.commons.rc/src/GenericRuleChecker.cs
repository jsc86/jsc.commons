// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2018 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

using jsc.commons.behaving;
using jsc.commons.behaving.interfaces;
using jsc.commons.misc;
using jsc.commons.rc.interfaces;

namespace jsc.commons.rc {

   public class GenericRuleChecker<T> : RuleCheckerBase<T> where T : class {

//, ICloneable{

      private readonly IBehaviors _context;
      private readonly bool _interactive;

      public GenericRuleChecker(
            IEnumerable<IRule<T>> rules = null,
            T subject = null,
            bool acceptInvalidSubject = false,
            IBehaviors context = null,
            bool interactive = false ) {
         _context = context??EmptyBehaviors.Instance;
         _interactive = interactive;

         if( rules != null )
            foreach( IRule<T> rule in rules )
               Add( rule );

         if( subject == null ) {
            ConstructorInfo ci;
            try {
               ci = typeof( T ).GetConstructor( Type.EmptyTypes );
            } catch( Exception exc ) {
               throw new ApplicationException(
                     $"{nameof( subject )} is null and there is no default ctor for {typeof( T ).FullName}",
                     exc );
            }

            Subject = (T)ci.Invoke( new object[0] );
         } else {
            Subject = subject;
         }

         ApplicationResult<T> ar = Apply(
               acceptInvalidSubject
                     ? (Func<IViolation<T>, ISolution<T>>)null
                     : v => throw new ApplicationException( "invalid subject in ctor" ) );
         if( ar.Successful )
            ar.Accept( );
         else
            throw new ApplicationException(
                  $"unsolvable violation for subject in ctor: {ar.UnsolvableViolation.Description}" );
      }

      public T Subject { get; protected set; }

      public virtual ApplicationResult<T> Apply( params IAction<T>[] actions ) {
         return Apply( null, actions );
      }

      public virtual ApplicationResult<T> Apply(
            Func<IViolation<T>, ISolution<T>> onViolation,
            params IAction<T>[] actions ) {
         ApplicationResult<T> applicationResult = null;
         PropertyObject<bool> cancellationToken = new PropertyObject<bool>( false );

         // TODO: proper clone handling (https://github.com/jsc86/jsc.commons/issues/10)
         T subjectClone = (T)( Subject as ICloneable )?.Clone( );
         foreach( IAction<T> action in actions )
            action.Apply( subjectClone, _context );

         ApplyInternal(
               subjectClone,
               new List<IAction<T>>( actions ),
               ar => {
                  applicationResult = ar;
                  cancellationToken.Value = true;
               },
               ( ) => { Subject = applicationResult.ResultSubject; },
               cancellationToken,
               onViolation??( v => v.Solutions.FirstOrDefault( ) ) );

         return applicationResult;
      }

      private void ApplyInternal(
            T subject,
            IList<IAction<T>> appliedActions,
            Action<ApplicationResult<T>> onResult,
            Action acceptDelegate,
            PropertyObject<bool> cancelationToken,
            Func<IViolation<T>, ISolution<T>> onViolation ) {
         IViolation<T> violation = Check( subject, _context );

         if( violation == NonViolation<T>.Instance ) {
            onResult(
                  new ApplicationResult<T>(
                        subject,
                        null,
                        new ReadOnlyCollection<IAction<T>>( appliedActions ),
                        acceptDelegate ) );
         } else {
            if( onViolation == null ) {
               foreach( ISolution<T> solution in violation.Solutions ) {
                  if( cancelationToken.Value )
                     return;

                  if( solution.Actions.Any(
                        a => ( _interactive||!a.IsInteractive )&&appliedActions.Any( a.Contradicts ) ) )
                     continue;

                  // TODO: proper clone handling (https://github.com/jsc86/jsc.commons/issues/10)
                  T subjectClone = (T)( subject as ICloneable )?.Clone( );
                  List<IAction<T>> appliedActionsClone = new List<IAction<T>>( appliedActions );
                  foreach( IAction<T> action in solution.Actions.Where( a => _interactive||!a.IsInteractive ) ) {
                     action.Apply( subjectClone, _context );
                     appliedActionsClone.Add( action );
                  }

                  ApplyInternal(
                        subjectClone,
                        appliedActionsClone,
                        onResult,
                        acceptDelegate,
                        cancelationToken,
                        null );
               }
            } else {
               ISolution<T> solution = onViolation( violation );
               if( solution == null ) {
                  cancelationToken.Value = true;
                  onResult(
                        new ApplicationResult<T>(
                              subject,
                              violation,
                              new ReadOnlyCollection<IAction<T>>( appliedActions ),
                              ( ) => { } ) );
               } else {
                  // TODO: proper clone handling (https://github.com/jsc86/jsc.commons/issues/10)
                  T subjectClone = (T)( subject as ICloneable )?.Clone( );
                  List<IAction<T>> appliedActionsClone = new List<IAction<T>>( appliedActions );
                  foreach( IAction<T> action in solution.Actions ) {
                     action.Apply( subjectClone, _context );
                     appliedActionsClone.Add( action );
                  }

                  ApplyInternal(
                        subjectClone,
                        appliedActionsClone,
                        onResult,
                        acceptDelegate,
                        cancelationToken,
                        onViolation );
               }
            }
         }
      } // ApplyInternal

   }

}
