// Licensed under the MIT license.
// See LICENSE file in the project root directory for full information.
// Copyright (c) 2021 Jacob Schlesinger
// File authors (in chronological order):
//  - Jacob Schlesinger <schlesinger.jacob@gmail.com>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using jsc.commons.behaving;
using jsc.commons.behaving.interfaces;
using jsc.commons.hierarchy.backend.exceptions;
using jsc.commons.hierarchy.backend.interfaces;
using jsc.commons.hierarchy.config;
using jsc.commons.hierarchy.localfs.config;
using jsc.commons.hierarchy.meta;
using jsc.commons.hierarchy.meta.interfaces;
using jsc.commons.hierarchy.resources;
using jsc.commons.hierarchy.resources.interfaces;
using jsc.commons.misc;

using YamlDotNet.Serialization;

using File = System.IO.File;
using Path = jsc.commons.hierarchy.path.Path;

namespace jsc.commons.hierarchy.localfs {

   public class LocalFsBackend : IHierarchyBackend {

      private readonly ILocalFsBackendConfiguration _config;

      private readonly IHierarchyConfiguration _hierarchyConfig;
      private Path _basePath;

      public LocalFsBackend(
            IHierarchyConfiguration hierarchyConfig,
            ILocalFsBackendConfiguration config ) {
         hierarchyConfig.MustNotBeNull( nameof( hierarchyConfig ) );
         config.MustNotBeNull( nameof( config ) );

         _hierarchyConfig = hierarchyConfig;
         _config = config;

         if( !_config.BasePath.Exists ) {
            if( _config.CreateBasePath )
               _config.BasePath.Create( );
            else
               throw new BackendInitializationException(
                     this,
                     $"base path {_config.BasePath.FullName} does not exist" );
         }
      }

      private Path BasePath => _basePath ??= Path.Parse( _config.BasePath.FullName );

      public void Dispose( ) {
         // nop
      }

      public Task<IResource> Get( Path path ) {
         path.MustNotBeNull( nameof( path ) );

         try {
            IMeta meta = null;
            string fullPath = BasePath.Append( path ).ToString( );
            DirectoryInfo diResource = new DirectoryInfo( fullPath );
            if( diResource.Exists ) {
               meta = GetMeta( new FileInfo( BasePath.Append( _config.MetaSuffix ).ToString( ) ) );
            } else {
               FileInfo fiResource = new FileInfo( fullPath );
               if( fiResource.Exists )
                  meta = GetMeta( new FileInfo( BasePath+_config.MetaSuffix ) );
            }

            if( meta == null )
               throw new BackendResourceNotFoundException( this, path, $"no file or folder {path} found" );

            BasicMeta basicMeta;
            if( !meta.TryGet( out basicMeta ) )
               throw new BackendMissingMetaException( this, path, $"missing meta information for path {path}" );

            IResourceClass resourceClass = _hierarchyConfig.KnownResourceClasses.FirstOrDefault(
                  rc => rc.Id.Equals( basicMeta.ResourceClass ) );

            if( resourceClass == null )
               throw new BackendUnknownResourceClassException(
                     this,
                     basicMeta.ResourceClass,
                     $"unknown resource class id {basicMeta.ResourceClass}" );

            return path.Equals( Path.RootPath )
                  ? Task.FromResult( (IResource)new Folder( Path.RootPath, null, meta ) )
                  : Task.FromResult( resourceClass.CreateResource( path.BasePath, path.Name, meta ) );
         } catch( BackendExceptionBase ) {
            throw;
         } catch( Exception exc ) {
            throw new BackendReadException( this, path, $"failed to get resource for path {path}", exc );
         }
      }

      public Task<IEnumerable<string>> List( Path path ) {
         path.MustNotBeNull( nameof( path ) );
         try {
            string fullPath = BasePath.Append( path ).ToString( );
            DirectoryInfo diResource = new DirectoryInfo( fullPath );
            if( !diResource.Exists )
               throw new BackendResourceNotFoundException( this, path, $"no folder {path} found" );

            string metaSuffix = _config.MetaSuffix;
            return Task.FromResult(
                  (IEnumerable<string>)
                  diResource.GetDirectories( )
                        .Select( di => di.Name )
                        .Union(
                              diResource.GetFiles( )
                                    .Where( fi => !fi.Name.EndsWith( metaSuffix ) )
                                    .Select( fi => fi.Name )
                        )
                        .ToList( ) );
         } catch( BackendExceptionBase ) {
            throw;
         } catch( Exception exc ) {
            throw new BackendReadException( this, path, $"failed to list resources for path {path}", exc );
         }
      }

      public Task Set( IResource resource ) {
         resource.MustNotBeNull( nameof( resource ) );

         try {
            if( resource is FolderResourceBase )
               SetFolder( resource );
            else
               SetFile( resource );

            return Task.CompletedTask;
         } catch( BackendExceptionBase ) {
            throw;
         } catch( Exception exc ) {
            throw new BackendWriteException( this, resource, $"failed to write resource {resource}", exc );
         }
      }

      public Task Delete( IResource resource ) {
         resource.MustNotBeNull( nameof( resource ) );

         try {
            if( resource is FolderResourceBase )
               DeleteFolder( resource );
            else
               DeleteFile( resource );

            return Task.CompletedTask;
         } catch( BackendExceptionBase ) {
            throw;
         } catch( Exception exc ) {
            throw new BackendDeleteException( this, resource, $"failed to delete resource {resource}", exc );
         }
      }

      public Task Move( IResource resource, Path targetPath ) {
         // TODO: impl
         throw new NotImplementedException( );
      }

      private void DeleteFile( IResource resource ) {
         string filePath = BasePath.Append( resource.Path ).ToString( );
         string metaPath = $"{filePath}{_config.MetaSuffix}";

         File.Delete( metaPath );
         File.Delete( filePath );
      }

      private void DeleteFolder( IResource resource ) {
         string folderPath = BasePath.Append( resource.Path ).ToString( );

         Directory.Delete( folderPath, true );
      }

      private IMeta GetMeta( FileInfo fiMeta ) {
         using( FileStream fs = fiMeta.OpenRead( ) )
         using( TextReader tr = new StreamReader( fs ) ) {
            IMeta meta = new Meta( );
            Deserializer deserializer = new Deserializer( );
            StringBuilder sb = new StringBuilder( );

            string line;
            while( !string.IsNullOrEmpty( line = tr.ReadLine( ) ) ) {
               Type metaType = Type.GetType( line );
               while( ( line = tr.ReadLine( ) ) != "---" )
                  sb.AppendLine( line );

               object metaObj = deserializer.Deserialize( sb.ToString( ), metaType );
               meta.Set( (IBehavior)metaObj );
            }

            return meta;
         }
      }

      private void SetFile( IResource file ) {
         string fullPath = BasePath.Append( file.Path ).ToString( );
         FileInfo fi = new FileInfo( fullPath );
         if( !fi.Exists ) // touch
            using( FileStream fs = fi.Create( ) ) {
               fs.Close( );
            }

         WriteMeta( file, new FileInfo( fullPath+_config.MetaSuffix ) );
      }

      private void SetFolder( IResource folder ) {
         string fullPath = BasePath.Append( folder.Path ).ToString( );
         DirectoryInfo di = new DirectoryInfo( fullPath );
         if( !di.Exists )
            di.Create( );

         WriteMeta( folder, new FileInfo( BasePath.Append( folder.Path ).Append( _config.MetaSuffix ).ToString( ) ) );
      }

      private void WriteMeta( IResource resource, FileInfo fiMeta ) {
         if( !resource.Meta.TryGet( out BasicMeta basicMeta ) ) {
            basicMeta = new BasicMeta {ResourceClass = resource.ResourceClass.Id};
            resource.Meta.Set( basicMeta );
         }

         using( FileStream fs = fiMeta.Open( FileMode.OpenOrCreate ) )
         using( TextWriter tw = new StreamWriter( fs ) ) {
            fs.SetLength( 0 ); // clear old content
            fs.Flush( );
            Serializer serializer = new Serializer( );
            foreach( object meta in resource.Meta.Objects( ) ) {
               tw.WriteLine( $"{meta.GetType( ).FullName}, {meta.GetType( ).Assembly.GetName( ).Name}" );
               // serializer.Serialize( tw, meta );
               // work around serializer bug
               string s = serializer.Serialize( meta );
               tw.Write( s.Replace( "\n\n", "\n" ) );
               tw.WriteLine( "---" );
            }

            tw.Flush( );
         }
      }

   }

}
