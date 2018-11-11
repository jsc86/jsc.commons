# jsc.commons.misc

This is the place for the misfits.
A misfit is
 - needed by more than one project
 - too small to get its own project

## EnumerableWrapper
If you don't want to implement IEnumerable in a class and/or
want to expose multiple collections for enumeration but
prevent their modification you can use the EnumerableWrapper:
```cs
class MyClass {
   List<object> _mySensitiveList = new List<object>( );

   public IEnumerable<object> Objects => new EnumerableWrapper<object>( _mySensitiveList );
}
```

## Lazy
What can *jsc.commons.Lazy\<T\>* do that
[System.Lazy\<T\>](https://docs.microsoft.com/en-us/dotnet/api/system.lazy-1?view=netframework-4.7.2)
can't? Absolutely nothing! And that's its charm; It's
boiled down to the essentials: No bloat/overhead for
serialization, marshalling or the likes. It's thread
safe, though.
```cs
class MyClass {
   private Lazy<MyExpensiveOptionalClass> _lazyExpensiveObject
         = new Lazy<MyExpensiveOptionalClass>(
               ( ) => new MyExpensiveOptionalClass( 42, "forty-two" ) );

   public bool HasExpensiveObjectInstance => _lazyExpensiveObject.IsInitialized;

   public MyExpensiveOptionalClass ExpensiveObject => _lazyExpensiveObject.Instance;
}
```

## PropertyObject
Because sometimes you just need to box something ...
for reasons; And it just does not justify a custom class
each time.
```cs
PropertyObject<bool> cancellationToken = new PropertyObject<bool>( false );
```
It has a Property *Value* of type T; That's it.
