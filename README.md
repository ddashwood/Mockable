# Mockable
## Simplifying dependncy injection into tests

Mockable was conceived as a result of a common difficulty with unit tests. When your
tests use the `new` keyword to create instances of the class under test, you specify
arguments for each constructor parameter. Often, these arguments are mocks of services -
services which, in the live application, would be provided by dependency injection, but
in a testing environment they need to be provided by the unit test itself.

Then, at some point later in time, new requirements mean that a new service is required
by the class under test. You add a new parameter to the constructor, and dependency
injection takes care of providing an argument for that parameter. But your unit tests
which use the `new` keyword no longer compile - they don't supply enough arguments for
the revised parameter list.

I've seen cases where adding a new dependency in this way has caused hundreds of tests
to fail to compile. In each case, the line which creates the class under test has needed
to be amended - often just adding a `null` to the end of the parameter list is enough
to make the test compile and pass again (but do we really want to pass `null` for many
of our services, jsut because this particular test happens to not use the service?)

Sure, we can simplify this situation by re-thinking how we write our unit tests. Factoring
the creation of the class under test into its own method often means that the number of
lines that needed to be amended when a new services is required can be minimised, for
example. But in the live system, we don't need to give any consideration to this issue,
because all of the dependencies are automatically generated and injected by the dependency
injection system. Wouldn't it be great if our unit tests could be just as flexible?

Well, now they can! Mockable is a simple framework for creating your class under test.
You tell it the data type you want, and it creates an instance of that data type -
including creating mocks of all of the constructor parameters, all automatically.
Making your unit tests compile when you change your dependency list is now just as
straightforward as making your production code compile. Of course, you may need to amend
the logic in the tests, and Mockable can't do that bit for you. But Mockable takes
the most painful part away from you and handles it all automatically.

# Show me some code!

Of course! Currently, Mockable supports making mocks with either Moq or FakeItEasy.
Add a reference to the Nuget package `Mockable.Moq` or `Mockable.FakeItEasy`, then
add code like this:

```csharp
using Mockable.Moq;

[Fact]
public void ExampleTest()
{
    var serviceFactory = new ServiceFactory();
    var objectUnderTest = serviceFactory.Create<ClassUnderTest>();
}
```

This will create an instance of `ClassUnderTest`. And if `ClassUnderTest` has a
constructor which needs parameters, it will create a mock for each parameter using
Moq. If you prefer FakeItEasy, just swap out the `using` statement at the top.

## What if I need to set up my Mock? Or verify calls on it?
### Introducing Configurators

Mockable can return the mock objects back to you, in whatever is the appropriate
format for the mocking library you've chosen to use. Mockable calls these mock
objects "configurators". The exact format of a configurator depends on the mocking
library used.

If you use FakeItEasy, the configurator is the same object as the mock. If you
weren't using Mockable, you might write code like this:

```csharp
var configurator = A.Fake<MyService>();
var mock = configurator; // Mock and Configurator are the same object
A.CallTo(() => configurator.Example()).Returns("Hello World!");
```

If you were using Moq, you would write very similar code, except that the mock
has to be obtained from the configurator using the `Object` property, as shown
here:

```csharp
var configurator = new Mock<MyService>();
var mock = configurator.Object; // Mock and Configurator are *not* the same object
configurator.Setup(m => m.Example()).Returns("Hello World!");
```

From these examples, you can see that the configurator is the object which the
mocking framework provides to you for configuring the mock. Whether the
configurator is the same object as the mock depends on the design of the mocking
framework being used.

### So how do I access Configurators?

To access configurators, simply create a class with properties for each
configurator:

```csharp
public class ClassUnderTest
{
    public ClassUnderTest(Service1 myService1, Service2 myService2, Service3 myService3)
    { ... }

    ...
}

// Test code below:

public class ClassUnderTestConfigurators
{
    public Mock<Service1> MyService1 { get; set; } = null!;
    public Mock<Service2> MyService2 { get; set; } = null!;
}

[Fact]
public void ExampleTest()
{
    var serviceFactory = new ServiceFactory();
    var objectUnderTest = serviceFactory.Create<ClassUnderTest, ClassUnderTestConfigurators>(out var configurators);

    configurators.Service1.Setup(m => ...).Returns(...);
}
```

A few things to note here:

- The data type of the properties depends on the mocking framework being used. In
the example, we assume that Moq is being used, and so we use data types such as
`Mock<Service1>` because that's the data type of a Moq configurator.
- You do not need to provide configurator properties for every parameter, only for
those you actually need to configure. In the example above, we have not provided a
configurator property for the third parameter, `myService3`.
- The name of the properties is important! It is PascalCased (Mockable takes the
name of the parameter, and converts the first alphabetic character in that name
to upper case). It must either be the same as the parameter name (except for the
case), or it must be the same name with the suffix `Configurator`. E.g. if the
parameter is called `myService2`, valid property names for the configurator
property would be either `MyService2` or `MyService2Configurator`

# Show me more!

Of course! The best thing to do if you want to see Mockable in action is download
the example code, which is part of the GitHub repo:

https://github.com/ddashwood/Mockable/tree/master/example

The example shows a simple ASP.Net MVC application, which asks the user to input
a date, and shows some information about the date - what day of the week it is,
and whether the date is a public holiday in the UK.

In order to do this, it includes a service (`DateService`) which finds all the
relevant information. `DateService` has a dependency on another service, an
implementation of `IBankHolidayService` - the real version of this interface
uses a public API provided by the UK government to check if a date is public
holiday (bank holiday) in the UK. But in order to test `DateService`, we
need mocks of `IBankHolidayService`.

The example includes tests for `DateService`, in which the `DateService`
instances are all created using Mockable. Mockable automatically creates
mocks of `IBankHolidayService`. Two copies of the unit tests are provided - one
using Moq, and one using FakeItEasy.