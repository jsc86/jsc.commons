// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

using jsc.commons.config;
using jsc.commons.config.interfaces;
using jsc.commons.hierarchy.acl.privileges.classes;
using jsc.commons.hierarchy.acl.privileges.classes.interfaces;
using jsc.commons.hierarchy.groups;
using jsc.commons.hierarchy.resources.classes;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.hierarchy.users;

namespace jsc.commons.hierarchy.config {

   [Config( DefaultsProvider = typeof( HierarchyConfigurationDefaultsProvider ) )]
   public interface IHierarchyConfiguration : IConfiguration {

      [ConfigValue]
      List<IPrivilegeClass> KnownPrivilegeClasses { get; set; }

      [ConfigValue]
      List<IResourceClass> KnownResourceClasses { get; set; }

      [ConfigValue]
      IBackendConfiguration BackendConfiguration { get; set; }

      [ConfigValue( Default = false )]
      bool AllowUseOfMoveFallback { get; set; }

      [ConfigValue]
      TraceHandler TraceHandler { get; set; }

   }

   public class HierarchyConfigurationDefaultsProvider : DefaultsProviderBase {

      public HierarchyConfigurationDefaultsProvider( ) : base(
            new[] {
                  new Tuple<string, Func<object>>(
                        nameof( IHierarchyConfiguration.KnownPrivilegeClasses ),
                        ( ) => new List<IPrivilegeClass> {
                              AllPrivilegeClass.Instance,
                              CreatePrivilegeClass.Instance,
                              DeletePrivilegeClass.Instance,
                              ReadPrivilegeClass.Instance,
                              WritePrivilegeClass.Instance,
                              GrantPrivilegeClass.Instance
                        } ),
                  new Tuple<string, Func<object>>(
                        nameof( IHierarchyConfiguration.KnownResourceClasses ),
                        ( ) => new List<IResourceClass> {
                              FolderResourceClass.Instance,
                              FileResourceClass.Instance,
                              GroupResourceClass.Instance,
                              UserResourceClass.Instance,
                              ReferenceResourceClass.Instance
                        } )
            } ) { }

   }

}
