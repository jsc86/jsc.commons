<img align="right" src="../../../img/logo/jsc.commons.logo_128.png"/>

# jsc.commons.naming
What does JSC_COMMONS_NAMING do? JscCommonsNaming
does what jsc-commons-naming does: It provides a way
to define names in a unified way and format and parse
them to and from different formats.
```cs
UnifiedName myPropertyName =
      NamingStyles.camelCase.FromString(
            nameof( myPropertyName ) );
Console.WriteLine( myPropertyName.ToString( ) );
Console.WriteLine( NamingStyles.SNAKE_CASE.ToString( myPropertyName ) );
// prints:
// my.property.name
// MY_PROPERTY_NAME
```
It can handle abbreviations to some degree.
Say you wanted to prefix your variables with
*MSA* (My Super Awesome) instead of just *my*;
In the above example *myPropertyName* would become
*MSAPropertyName* and would be represented as the
unified name *MSA.property.name*. Formatting to different
naming styles works just fine - the information of
*MSA* being an abbreviation can get lost with certain
styles and not recovered when parsing. For example
there is no easy way to tell that *msa* is an abbreviation
in *msa_property_name*.
