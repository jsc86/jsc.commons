// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2020 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using jsc.commons.hierarchy.acl.interfaces;
using jsc.commons.hierarchy.acl.privileges.classes.interfaces;
using jsc.commons.hierarchy.acl.privileges.interfaces;
using jsc.commons.hierarchy.config;
using jsc.commons.hierarchy.path;
using jsc.commons.hierarchy.path.interfaces;
using jsc.commons.misc;

using Enumerable = System.Linq.Enumerable;

namespace jsc.commons.hierarchy.acl {

   public abstract class AccessControlRule : IAccessControlRule {

      private static readonly Regex __parseRegex =
            new Regex( "(?<action>(DENY)|(ALLOW)) (?<privileges>(.+,?)+) TO ((?<target_prefix>.+) )?(?<target>.+)" );

      private string _stringRepresentation;

      protected AccessControlRule(
            EnAccessControlAction action,
            IPath toPath,
            IEnumerable<IPrivilege> privileges ) {
         Action = action;
         ToPath = toPath;
         Privileges = privileges == null
               ? Enumerable.Empty<IPrivilege>( )
               : new ReadOnlyCollection<IPrivilege>( privileges.ToList( ) );
      }

      protected virtual string ToPrefix { get; } = string.Empty;

      public EnAccessControlAction Action { get; }
      public IPath ToPath { get; }
      public IEnumerable<IPrivilege> Privileges { get; }

      public override string ToString( ) {
         if( _stringRepresentation == null ) {
            StringBuilder sb = new StringBuilder( );
            ToString( sb );
            _stringRepresentation = sb.ToString( );
         }

         return _stringRepresentation;
      }

      public virtual void ToString( StringBuilder sb ) {
         if( _stringRepresentation != null ) {
            sb.Append( _stringRepresentation );
            return;
         }

         sb.Append( Action.ToString( ).ToUpper( ) );
         sb.Append( ' ' );
         foreach( IPrivilege privilege in Privileges ) {
            sb.Append( privilege );
            sb.Append( ", " );
         }

         sb.Remove( sb.Length-2, 2 );
         sb.Append( " TO " );
         sb.Append( ToPrefix );
         sb.Append( ToPath );
      }

      public static IAccessControlRule Parse( string acrString, IHierarchyConfiguration config ) {
         acrString.MustNotBeNull( nameof( acrString ) );

         acrString = acrString.Trim( );
         if( acrString.ToUpper( ) == EnAccessControlAction.Inherit.ToString( ).ToUpper( ) )
            return new AcrInherit( );

         Match match = __parseRegex.Match( acrString );
         if( !match.Success )
            throw new Exception( $"malformed ACR '{acrString}'" );

         string actionString = match.Groups[ "action" ].Value.ToUpper( );
         EnAccessControlAction action;
         switch( actionString ) {
            case "ALLOW":
               action = EnAccessControlAction.Allow;
               break;
            case "DENY":
               action = EnAccessControlAction.Deny;
               break;
            default:
               throw new Exception( $"unknown ACR action '{actionString}'" );
         }

         List<IPrivilege> privileges = new List<IPrivilege>( );
         foreach( string privilegeString in match.Groups[ "privileges" ].Value.Split( ',' ).Select( p => p.Trim( ) ) ) {
            IPrivilegeClass privilegeClass =
                  config.KnownPrivilegeClasses.FirstOrDefault(
                        pc => string.Compare(
                                    pc.Name,
                                    privilegeString,
                                    StringComparison.InvariantCultureIgnoreCase )
                              == 0 );

            if( privilegeClass == null )
               throw new Exception( $"unknown ACR privilege class '{privilegeString}'" );

            privileges.Add( privilegeClass.CreatePrivilege( ) );
         }

         string targetPrefix = null;

         if( match.Groups[ "target_prefix" ].Success ) {
            targetPrefix = match.Groups[ "target_prefix" ].Value.Trim( ).ToUpper( );
            switch( targetPrefix ) {
               case "USER":
               case "GROUP":
                  break;
               default:
                  throw new Exception( $"unknown ACR target prefix '{targetPrefix}'" );
            }
         }

         IPath target = null;
         string targetString = match.Groups[ "target" ].Value.Trim( );

         if( targetString.ToUpper( ) == "EVERYONE" )
            targetPrefix = "EVERYONE";
         else
            try {
               target = Path.Parse( targetString );
            } catch( Exception ) {
               throw new Exception( $"malformed target '{targetString}'" );
            }

         return targetPrefix switch {
               "USER" => new AcrUser( action, target, privileges ),
               "GROUP" => new AcrGroup( action, target, privileges ),
               "EVERYONE" => new AcrEveryone( action, privileges ),
               _ => throw new Exception( ) // make the compiler happy, should be dead code
         };
      }

   }

}