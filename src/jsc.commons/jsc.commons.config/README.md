<img align="right" src="../../../img/logo/jsc.commons.logo_128.png"/>

# jsc.commons.config

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

```cs
public class MyConfDefaultsProvider : DefaultsProviderBase {

   public MyConfDefaultsProvider( ) : base(
         new[] {
               new Tuple<string, object>(
                     nameof( IMyConf.MyListProp ),
                     new List<string> {"a", "b", "c"} ),
         } ) { }
}
```

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
