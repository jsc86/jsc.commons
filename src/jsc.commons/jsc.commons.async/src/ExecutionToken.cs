// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Threading.Tasks;

namespace jsc.commons.async {

   public sealed partial class ExecutionTokenSpool {

      public sealed class ExecutionToken : IDisposable {

         private readonly ExecutionTokenSpool _execTokenSpool;

         internal ExecutionToken( ExecutionTokenSpool execTokenSpool ) {
            _execTokenSpool = execTokenSpool;
         }

         internal TaskCompletionSource<ExecutionToken> Tcs { get; } = new TaskCompletionSource<ExecutionToken>( );

         internal ExecutionToken Next { get; set; }

         public void Dispose( ) {
            ReleaseExecutionToken( );
            GC.SuppressFinalize( this );
         }

         private void ReleaseExecutionToken( ) {
            // ReSharper disable once InconsistentlySynchronizedField
            if( _execTokenSpool._disposed )
               return;

            lock( _execTokenSpool._lockObj ) {
               if( _execTokenSpool._disposed )
                  return;

               if( Next == null ) {
                  _execTokenSpool._currentExecToken = null;
                  _execTokenSpool._lastExecutionToken = null;
               } else {
                  _execTokenSpool._currentExecToken = Next;
                  Next.Tcs.SetResult( Next );
               }
            }
         }

         ~ExecutionToken( ) {
            ReleaseExecutionToken( );
         }

      }

   }

}
