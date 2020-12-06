// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2019 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using jsc.commons.unidto.core.interfaces;

namespace jsc.commons.unidto.tests {

   public interface ITestInterface {

      int? Prop { get; set; }

   }

   public interface ITestInterfaceSdc : ITestInterface, IDirty { }

   public interface ITestInterfaceAdc : ITestInterface, IChangeable { }

   public interface ITestInterfaceVdc : ITestInterface, IVersioned { }

   public interface ITestInterfaceSdcNpc : ITestInterface, IDirty, INotifyPropertyChanged { }

   public interface ITestInterfaceAdcNpc : ITestInterface, IChangeable, INotifyPropertyChanged { }

   public interface ITestInterfaceVdcNpc : ITestInterface, IVersioned, INotifyPropertyChanged { }

}