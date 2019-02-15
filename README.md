<img align="right" src="img/logo/jsc.commons.logo_128.png"/>

# jsc.commons


## [jsc.commons.behaving](src/jsc.commons/jsc.commons.behaving/)
*Behaving* is a codified pattern to add common behaviors
to a set of instances which generally know how to behave
(pun intended).

## [jsc.commons.cli](src/jsc.commons/jsc.commons.cli/)
jsc.commons.cli is a framework for implementing Command Line Interfaces (cli).
Why a framework? Because it is more than just a command line parser.
It supports constraints (i.e. if option A is set, option B must not be set),
provides interactive correction of invalid command lines, help printing ...

## [jsc.commons.config](src/jsc.commons/jsc.commons.config/)
A recurring task in software development is writing code for
configuration objects, which alter the way a software behaves.
Often this will be technology specific code, for example using
the app.config or a specific db schema. This library provides
a way to define technology independent configuration objects
with the IO abstracted away. Write the configuration object
code once, replace the IO source/destination late without
changing the configuration object code.

## [jsc.commons.misc](src/jsc.commons/jsc.commons.misc/)
This is the place for the misfits.
A misfit is
 - needed by more than one project
 - too small to get its own project
 
## [jsc.commons.naming](src/jsc.commons/jsc.commons.naming/)
What does JSC_COMMONS_NAMING do? JscCommonsNaming
does what jsc-commons-naming does: It provides a way
to define names in a unified way and format and parse
them to and from different formats.

## [jsc.commons.rc](src/jsc.commons/jsc.commons.rc/)
The Rule Checker (rc) can be used to solve
[constraint satisfaction problems](https://en.wikipedia.org/wiki/Constraint_satisfaction_problem).
