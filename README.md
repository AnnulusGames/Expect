# Expect  
 Expression-based Assertion Library for .NET  

[![NuGet](https://img.shields.io/nuget/v/Expect.svg)](https://www.nuget.org/packages/Expect)  
[![Releases](https://img.shields.io/github/release/AnnulusGames/Expect.svg)](https://github.com/AnnulusGames/Expect/releases)  
[![GitHub license](https://img.shields.io/github/license/AnnulusGames/Expect.svg)](./LICENSE)  

[English](./README.md) | 日本語  

**Expect** is an assertion library for .NET.  

The concept of Expect is inspired by [power-assert](https://github.com/power-assert-js/power-assert) and [swift-testing](https://github.com/swiftlang/swift-testing), offering better assertions using Expression Trees.  

## Why Expect?  

In test frameworks like NUnit, assertions are written using `Assert.That`. Below is an example of a unit test using NUnit:  

```cs
// Define a record for testing
record Person(string Name, int Age);
```

```cs
[Test]
public void Example()
{
    var person = new Person("Alice", 16);
    Assert.That(person.Age, Is.GreaterThanOrEqualTo(18));   
}
```

This test will fail, displaying the following message:  

```
Expected: greater than or equal to 18
But was:  16
```

The problem with assertions using `Assert.That` is that users are required to distinguish between a large number of `Is.**` usages. Many test frameworks provide a vast number of APIs to cover all use cases, forcing users to memorize them.  

Some assertion libraries adopt a Fluent Interface, such as `foo.Should().Not.Be.Null()`. However, I believe such APIs should not be used, as they reduce readability. Overly verbose Fluent Interfaces, modeled after natural language, are not necessarily easy to read. Tests should not aim to mimic natural language but instead be programmer-friendly and written in readable code.  

With Expect, there is only one API provided:  

```cs
using static Expect.Expectation;

[Test]
public void Example()
{
    var person = new Person("Alice", 16);

    // Pass a condition
    Expect(() => person.Age >= 18);
}
```

This displays the following message:  

```
(person.Age → 16) >= 18
```

Expect analyzes the provided expression to display an appropriate error message.  

Below is a comparison of collection assertions:  

```cs
int[] array = [1, 2, 3, 4, 5];

// NUnit
Assert.That(array, Has.Member(-1));

// Expect
Expect(() => array.Contains(-1));
```

The error messages will be as follows:  

```
Expected: some item equal to -1 and some item equal to 1
But was:  < 1, 2, 3, 4, 5 >
```

```
(array → [1, 2, 3, 4, 5]).Contains(-1)
```

## Installation  

### NuGet packages  

Expect requires .NET Standard 2.0 or higher. The package is available via NuGet.  

### .NET CLI  

```ps1
dotnet add package Expect
```

### Package Manager  

```ps1
Install-Package Expect
```

## License  

This library is released under the [MIT License](LICENSE).  
