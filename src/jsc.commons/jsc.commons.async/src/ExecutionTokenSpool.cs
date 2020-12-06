// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Threading.Tasks;

namespace jsc.commons.async {

   public sealed partial class ExecutionTokenSpool : IDisposable {

      private readonly object _lockObj = new object( );
      private ExecutionToken _currentExecToken;

      private volatile bool _disposed;
      private ExecutionToken _lastExecutionToken;

      public void Dispose( ) {
         SetExecTokensToFailedState( );
         GC.SuppressFinalize( this );
      }

      public Task<ExecutionToken> GetExecutionToken( ) {
         lock( _lockObj ) {
            if( _disposed )
               throw new ExecutionTokenSpoolDisposedException( );

            ExecutionToken execToken = new ExecutionToken( this );

            if( _currentExecToken == null ) {
               _currentExecToken = execToken;
               _lastExecutionToken = execToken;
               execToken.Tcs.SetResult( execToken );
            } else {
               _lastExecutionToken.Next = execToken;
               _lastExecutionToken = execToken;
            }

            return execToken.Tcs.Task;
         }
      }

      private void SetExecTokensToFailedState( ) {
         _disposed = true;
         if( _currentExecToken == null )
            return;

         lock( _lockObj ) {
            if( _currentExecToken == null )
               return;

            ExecutionToken current = _currentExecToken;
            do {
               current.Tcs.TrySetException( new ExecutionTokenSpoolDisposedException( ) );
            } while( ( current = current.Next ) != null );
         }
      }

      ~ExecutionTokenSpool( ) {
         SetExecTokensToFailedState( );
      }

   }

   public sealed class ExecutionTokenSpoolDisposedException : Exception { }

}