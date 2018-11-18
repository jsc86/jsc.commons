<img align="right" src="../../../img/logo/jsc.commons.logo_128.png"/>

# jsc.commons.config
A recurring task in software development is writing code for
configuration objects, which alter the way a software behaves.
Often this will be technology specific code, for example using
the app.config or a specific db schema. This library provides
a way to define technology independent configuration objects
with the IO abstracted away. Write the configuration object
code once, replace the IO source/destination late without
changing the configuration object code.

The following snippet illustrates how to define a configuration
object. Write an interface extending IConfiguration. Each property
translates to a configuration setting. Configurations can be
annotated with the Config and ConfigValue attributes. The ConfigValue
attribute is mandatory to tell the generic configuration implementation
that a property is a configuration setting. It also allows to
define a default value.
```cs
[Config( DefaultsProvider = typeof( MyConfDefaultsProvider ) )]
public interface IMyConf : IConfiguration {

   [ConfigValue( Default = "my string value" )]
   string MyStringProperty { get; set; }

   [ConfigValue( Default = 42 )]
   int MyIntProp { get; set; }

   [ConfigValue]
   IList<string> MyListProp { get; set; }

}
```

Default values defined with the ConfigValue attribute have to be constant.
Everything you can define as a constant in C# can be used here.
If you need to set something as a default value which can not be
defined as a constant, you need to specify and write a defaults provider.
A default provider maps the name of a configuration property to
a function returning an instance as a default value.
```cs
public class MyConfDefaultsProvider : DefaultsProviderBase {

   public MyConfDefaultsProvider( ) : base(
         new[] {
               new Tuple<string, Func<object>>(
                     nameof( IMyConf.MyListProp ),
                     ( ) => new List<string> {"a", "b", "c"} )
         } ) { }  
}
```

Getting an instance of a configuration object is as simple as
calling Config.New\<T\>() where T is the interface definition of the
configuration object. The returned instance will be pre-filled with
the defined default values.
```cs
IMyConf conf = Config.New<IMyConf>( );
Console.WriteLine( conf.MyStringProperty );
Console.WriteLine( conf.MyIntProp );
Console.WriteLine( conf.MyListProp.Aggregate( ( a, b ) => $"{a}, {b}" ) );
// output:
// my string value
// 42
// a, b, c
```


Writing a configuration back-end can be achieved by implementing
the IConfigurationBackend interface. It is advised though, to
extend the ConfigBackendBase class, which provides some basic
error handling and a template for common IO operations.
```cs
public class MyPseudoBackEnd : ConfigBackendBase {

   protected override bool Read( string key, Type type, out object value ) {
      // this is a write only configuration backend ;-)
      throw new NotImplementedException( );
   }

   protected override void Save( string key, Type type, object value ) {
      Console.WriteLine( $"save '{key}': {value}" );
   }

   protected override void BeforeRead( ) {
      // this would be the place to open a file stream or db connection
   }

   protected override void AfterRead( ) {
      // this would be the place to close a file stream or db connection
   }

   protected override void OnReadException( Exception exc ) {
      // if anything fails while reading a configuration this method gets invoked
      // this is not the place for clean up, AfterRead gets called anyway
   }

   protected override void BeforeSave( ) {
      // this would be the place to open a file stream, db connection and/or create a transaction
   }

   protected override void AfterSave( ) {
      // this would be the place to close a file stream, db connection and/or commit a transaction
   }

   protected override void OnSaveException( Exception exc ) {
      // if anything fails while saving a configuration this method gets invoked
      // you might want to roll back a transaction here
      // AfterSave gets called anyway
   }

}
```

```cs
IMyConf conf = Config.New<IMyConf>( );
conf.Backend = new MyPseudoBackEnd( );
conf.MyStringProperty = "asdf";
conf.MyIntProp = 23;
conf.Save( );
// output
// save 'MyStringProperty': asdf
// save 'MyIntProp': 23
// save 'MyListProp': System.Collections.Generic.List`1[System.String]
```
