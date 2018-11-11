<img align="right" src="../../../img/logo/jsc.commons.logo_128.png"/>

# jsc.commons.behaving

*Behaving* is a codified pattern to add common behaviors
to a set of instances which generally know how to behave
(pun intended).

It is, for example, quite common for objects to have
a name or a description. A good practice is to extract
a string property *Name* into an interface *INameable*
and implement that everywhere where you want objects
to have a name.
*Behaving* essentially gives you the same ability plus
a bit more. The main difference is, that *Behaving*
allows you to give the behavior *INameable* to *instances*
of classes without the classes needing to implement
*INameable*.
```cs
foreach( object o in myList ) {
   if( o.TryGet( out INameble nameble ) ) {
      // do something with nameable
   }
}
```
This allows you to have a set of instances for a certain
class which features a behavior while another set does not.
It also allows to write generic code utilizing a behavior
on a set of instances of different classes.

Making your own class behave is as simple as extending
*BehaviorsContainerBase* or implementing *IBehaviorsContainer*.
```cs
interface INameable : IBehavior {
   string Name { get; set; }
}

class Nameable : INameable {
   public Nameable( string name ) {
      Name = name;
   }

   public string Name { get; set; }
}

class MyClass : BehaviorsContainerBase { }

static void Main( string[] args ) {
   MyClass myInstance = new MyClass( );
   myInstance.Set( new Nameable( "Hans" ) );
}
```
