<img align="right" src="../../../img/logo/jsc.commons.logo_128.png"/>

# jsc.commons.cli


```cs
[CliDefinition( Description = "a fine description" )]
public interface ICli : IConfiguration {

   [Option( Description = "first option", Flag = 'o' )]
   string OptionOne { get; set; }

   [Option( Description = "second option", Flag = 't', Optional = false )]
   int OptionTwo { get; set; }

   [Flag( Description = "a nice flag", Name = 'f' )]
   bool Flag { get; set; }

   [Argument( Description = "a superb argument" )]
   string MyAttrib { get; set; }

}
```

```cs
private static void Main( string[] args ) {
   ICli cli = new InterfaceSpecBoilerPlateHelper<ICli>( args ).CliConfigObject;
   Console.WriteLine( $"{nameof( ICli.OptionOne )}: {cli.OptionOne??"[null]"}" );
   Console.WriteLine( $"{nameof( ICli.OptionTwo )}: {cli.OptionTwo}" );
   Console.WriteLine( $"{nameof( ICli.Flag )}: {cli.Flag}" );
   Console.WriteLine( $"{nameof( ICli.MyAttrib )}: {cli.MyAttrib??"[null]"}" );
}
```

```
$ dotnet jsc.commons.playground.dll -o asdf   

--option-one asdf  
> violated: option option.two is set
1        add option option.two
2        auto solve
3        cancel
enter option: 2
OptionOne: asdf
OptionTwo: 0
Flag: False
MyAttrib: [null]

$ dotnet jsc.commons.playground.dll -o asdf -t 42 qwerty
OptionOne: asdf
OptionTwo: 42
Flag: False
MyAttrib: qwerty

$ dotnet jsc.commons.playground.dll -o asdf -t 42 qwerty -f
OptionOne: asdf
OptionTwo: 42
Flag: True
MyAttrib: qwerty

$ dotnet jsc.commons.playground.dll -o asdf -t 42 -f qwerty
OptionOne: asdf
OptionTwo: 42
Flag: True
MyAttrib: qwerty

$ dotnet jsc.commons.playground.dll qwerty -f -t 42 -o asdf
OptionOne: asdf
OptionTwo: 42
Flag: True
MyAttrib: qwerty

$ dotnet jsc.commons.playground.dll qwerty -ft 42          
OptionOne: [null]
OptionTwo: 42
Flag: True
MyAttrib: qwerty
```
