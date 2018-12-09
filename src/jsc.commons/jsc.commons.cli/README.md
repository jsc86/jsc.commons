<img align="right" src="../../../img/logo/jsc.commons.logo_128.png"/>

# jsc.commons.cli
jsc.commons.cli is a framework for implementing Command Line Interfaces (cli).
Why a framework? Because it is more than just a command line parser.
It supports constraints (i.e. if option A is set, option B must not be set),
provides interactive correction of invalid command lines, help printing ...

There are essentially two ways to use jsc.commons.cli:
 1. Define the CLI specification by code (this is more flexible)
 2. Define the CLI specification by interface (easier, zero boiler plate code)

The following examples merely scratch on the surface of the features
provided my jsc.commons.cli. But they should convey a good idea of
how to utilize it.

## CLI specification by code
The first example shows how to define and use a CLI specification by code: 
```cs
private static void Main( string[] args ) {
   ICliConfig conf = Config.New<ICliConfig>( );
   IArgument<int> intArg = new IntArg(
         "int", // name of the argument
         conf, // reference to an ICliConfig
         0, // minimum value (optional)
         10 // maximum value (optional)
   );
   ICliSpecification spec = new CliSpecification(
         conf, // reference to an ICliConfig (optional)
         null, // dynamic argument (optional)
         intArg // params IArgument[]
   );
   IFlag flag = new Flag( 'f' );
   spec.Flags.Add( flag );
   IArgument<string> stringArg = new StringArg(
         "string", // name of the argument
         false // argument is not optional
   );
   IOption opt = new Option(
         new UnifiedName( "my", "option" ),
         true, // option is optional
         'o', // option as flag alias o
         null, // dynamic argument (optional)
         stringArg // params IArgument[]
   );
   spec.Options.Add( opt );

   try {
      ICommandLineParser clp = new CommandLineParser( spec );
      IParserResult pr = clp.Parse( args );
      if( pr.IsSet( spec.HelpOption ) )
         throw new Exception( "show help" );
      ConflictResolver cr = new ConflictResolver( spec );
      if( cr.Resolve( pr, out pr, new BehaviorsBase( ) ) ) {
         // pr is a valid parser result, you can do something with it now
         if( pr.IsSet( intArg ) )
            Console.WriteLine( $"{intArg.Name}: {pr.GetValue( intArg )}" );
         if( pr.IsSet( flag ) )
            Console.WriteLine( $"{flag.Name} is set" );
         if( pr.IsSet( opt ) )
            Console.WriteLine( $"{opt.Name} is set, {stringArg.Name}: {pr.GetValue( stringArg )}" );
      }
   } catch( Exception exc ) {
      Console.WriteLine( exc.Message );
      IHelpPrinter hp = new TextHelpPrinter( spec );
      hp.Print( );
      Environment.Exit( -1 );
   }
}
```
Toying around with the above example in the command line:
```
$ dotnet jsc.commons.playground.dll                 
$ dotnet jsc.commons.playground.dll -h
show help
  -h, --help 
  -o, --my-option  string
  -f 

$ dotnet jsc.commons.playground.dll --help
show help
  -h, --help 
  -o, --my-option  string
  -f 

$ dotnet jsc.commons.playground.dll --my-option

--my-option   
> violated: (option help is set or (option my.option is set) implies (argument string is set))
1        add argument string
2        remove option my.option
3        auto solve
4        cancel
enter option: 2
$ dotnet jsc.commons.playground.dll -o asdf    
my.option is set, string: asdf

$ dotnet jsc.commons.playground.dll 42     
  
> violated: (option help is set or jsc.commons.cli.arguments.IntArg >= 10)
1        set argument int to 10
2        enter value for argument int
3        auto solve
4        cancel
enter option: 2

Enter value for argument int: 5
int: 5

# the following commands all create the same output
$ dotnet jsc.commons.playground.dll 7 -f --my-option asdf
$ dotnet jsc.commons.playground.dll 7 -f -o asdf
$ dotnet jsc.commons.playground.dll 7 -fo asdf
$ dotnet jsc.commons.playground.dll -fo asdf 7
int: 7
f is set
my.option is set, string: asdf

```

## CLI specification by interface
The following example shows how to use an interface to define a
CLI specification. As of now, interface specifications do not support
constraints (except for optional flag).
```cs
[CliDefinition( Description = "a very fine CLI specification" )]
public interface ICli : IConfiguration {

   [Help]
   bool HelpOption { get; set; }

   [Flag( Name = 'f', Description = "a very nice flag" )]
   bool Flag { get; set; }

// shorthand for an option with a string argument:
// [Option(Description = "a very nice option", Flag = 'o' )]
// [FirstArgument(Description = "a nice string argument")]
// string MyOption { get; set; }

   [Option( Description = "a very nice option", Flag = 'o' )]
   bool MyOption { get; set; }

   [Argument( Of = nameof( MyOption ), Optional = false, Description = "a nice string argument" )]
   string MyOptionString { get; set; }

   [Argument( Description = "an integer argument" )]
   int? IntArg { get; set; }

}

internal class Program {

   private static void Main( string[] args ) {
      InterfaceSpecBoilerPlateHelper<ICli> isbph = null;
      try {
         isbph = new InterfaceSpecBoilerPlateHelper<ICli>( args );
         ICli cli = isbph.CliConfigObject;
         if( cli.HelpOption )
            throw new Exception( "show help" );
         if( cli.IntArg.HasValue )
            Console.WriteLine( $"{nameof( cli.IntArg )}: {cli.IntArg}" );
         if( cli.Flag )
            Console.WriteLine( $"{nameof( cli.Flag )} is set" );
         if( cli.MyOption )
            Console.WriteLine( $"{nameof( cli.MyOption )} is set, string arg: {cli.MyOptionString}" );
      } catch( Exception exc ) {
         Console.WriteLine( exc.Message );
         if( isbph != null )
            new TextHelpPrinter( isbph.CliSpecification ).Print( );
         Environment.Exit( -1 );
      }
   }

}
```
Following is the help output on the command line. The rest of the
behavior is the same as in the previous example.
```
$ dotnet jsc.commons.playground.dll -h
show help
a very fine CLI specification
  -h, --help 
  -o, --my-option  String
               a very nice option
          String: a nice string argument
  -f 
               a very nice flag

  IntArg: an integer argument

```
