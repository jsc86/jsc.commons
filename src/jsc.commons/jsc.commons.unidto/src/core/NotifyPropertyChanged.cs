// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using INotifyPropertyChanged = jsc.commons.unidto.core.interfaces.INotifyPropertyChanged;

namespace jsc.commons.unidto.core {

   public sealed class NotifyPropertyChanged : INotifyPropertyChanged {

      private readonly Func<object> _getInstance;

      private object _instance;

      public NotifyPropertyChanged( Func<object> getInstance ) {
         _getInstance = getInstance;
      }

      private object Instance => _instance??( _instance = _getInstance( ) );

      public event PropertyChangedEventHandler PropertyChanged;

      internal void OnPropertyChanged( [CallerMemberName] string propertyName = null ) {
         PropertyChanged?.Invoke( Instance, new PropertyChangedEventArgs( propertyName ) );
      }

   }

}