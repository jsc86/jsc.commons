<img align="right" src="../../../img/logo/jsc.commons.logo_128.png"/>

# jsc.commons.rc

The Rule Checker (rc) can be used to solve
[constraint satisfaction problems](https://en.wikipedia.org/wiki/Constraint_satisfaction_problem).
An example says more than a thousand words
(or something along the lines):
```cs
GenericRuleChecker<IList<string>> grc = new GenericRuleChecker<IList<string>>(
      new[] {                  // rules
            new Implies<IList<string>>(
                  new Contains<string>( "X" ),
                  new Contains<string>( "Y" ) )
      },
      new CList<string> {"X"}, // subject
      true );                  // accept invalid subject (and make it valid)
Console.WriteLine( grc.Subject.Aggregate( ( a, b ) => $"{a}, {b}" ) );
// output: X, Y
```
Core elements:
 - a set of rules (in the example X ⇒ Y)
 - a subject which needs to be cloneable (in the example *CList*)
 - a *IRuleChecker* implementation (in the example *RuleCheckerBase*,
   encapsulated by *GenericRuleChecker*)

Each rule (*IRule* implementation) has a *Check* method.
Given a subject it either returns a violation (implementing
*IViolation*) or *NonViolation.Instance*.
A violation can have zero up to arbitrary many solutions
(implementing *ISolution*). A solution has a list of one or
more actions (implementing *IAction*). An action has
an *Apply* which can modify a given subject.

A *IRuleChecker* has two modes for checking rules:
 1. Check the subject with all rules until the first
    violation occurs and return it. If no rule is violated
    *NonViolation.Instance* is returned.
 2. Perform a full check of all rules on the subject
    and return a list of all violations (out parameter).
    This method returns true if there were no violations,
    otherwise false.

Rules can be interactive (requiring user input).
Interactive rules can not be used for automatic solving
of violations.

The Rule Checker is written in a rather generic and
easily extensible way; It is for example one of the
core components for [jsc.commons.cli](src/jsc.commons/jsc.commons.cli/).
