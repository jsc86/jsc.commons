// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;

using jsc.commons.config;
using jsc.commons.config.interfaces;
using jsc.commons.hierarchy.acl.interfaces;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.hierarchy.users;

namespace jsc.commons.hierarchy.config {

   [Config( DefaultsProvider = typeof( HierarchyManagerConfigurationDefaultsProvider ) )]
   public interface IHierarchyManagerConfiguration : IConfiguration {

      [ConfigValue]
      IPath BaseFolder { get; set; }

      [ConfigValue]
      IPath UsersFolder { get; set; }

      [ConfigValue]
      IPath GroupsFolder { get; set; }

      [ConfigValue( Default = "sysusr" )]
      string SystemUser { get; set; }

      [ConfigValue]
      List<IPath> ExcludePaths { get; set; }

      [ConfigValue]
      Action<IAccessControlList, User> BaseFolderAclFactory { get; set; }

   }

   public class HierarchyManagerConfigurationDefaultsProvider : DefaultsProviderBase {

      public HierarchyManagerConfigurationDefaultsProvider( ) :
            base(
                  new[] {
                        new Tuple<string, Func<object>>(
                              nameof( IHierarchyManagerConfiguration.BaseFolder ),
                              ( ) => new Path( true ) ),
                        new Tuple<string, Func<object>>(
                              nameof( IHierarchyManagerConfiguration.UsersFolder ),
                              ( ) => new Path( "users", false ) ),
                        new Tuple<string, Func<object>>(
                              nameof( IHierarchyManagerConfiguration.GroupsFolder ),
                              ( ) => new Path( "groups", false ) ),
                        new Tuple<string, Func<object>>(
                              nameof( IHierarchyManagerConfiguration.ExcludePaths ),
                              ( ) => new List<IPath>( ) )
                  } ) { }

   }

}