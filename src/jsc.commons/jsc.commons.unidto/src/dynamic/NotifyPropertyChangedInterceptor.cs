using System;
using System.Reflection;

using Castle.DynamicProxy;

using jsc.commons.unidto.core;

namespace jsc.commons.unidto.dynamic {

   public class NotifyPropertyChangedInterceptor : IInterceptor {

      private static readonly MethodInfo __miAddPropertyChanged;
      private static readonly MethodInfo __miRemPropertyChanged;

      private readonly NotifyPropertyChanged _npc;

      static NotifyPropertyChangedInterceptor( ) {
         EventInfo ei = typeof( NotifyPropertyChanged ).GetEvent( nameof( NotifyPropertyChanged.PropertyChanged ) );
         __miAddPropertyChanged = ei.AddMethod;
         __miRemPropertyChanged = ei.RemoveMethod;
      }

      public NotifyPropertyChangedInterceptor( NotifyPropertyChanged npc ) {
         _npc = npc;
      }

      public void Intercept( IInvocation invocation ) {
         if( invocation.Method.Name == __miAddPropertyChanged.Name )
            invocation.ReturnValue = __miAddPropertyChanged.Invoke( _npc, invocation.Arguments );
         else if( invocation.Method.Name == __miRemPropertyChanged.Name )
            invocation.ReturnValue = __miRemPropertyChanged.Invoke( _npc, invocation.Arguments );
         else
            throw new Exception( $"{invocation.Method.Name} is not a NotifyPropertyChanged method" );
      }

   }

}