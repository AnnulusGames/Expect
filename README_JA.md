# Expect
 Expression-based Assertion Library for .NET

[![NuGet](https://img.shields.io/nuget/v/Expect.svg)](https://www.nuget.org/packages/Expect)
[![Releases](https://img.shields.io/github/release/AnnulusGames/Expect.svg)](https://github.com/AnnulusGames/Expect/releases)
[![GitHub license](https://img.shields.io/github/license/AnnulusGames/Expect.svg)](./LICENSE)

[English]((./README.md)) | 日本語

Expectは.NET向けのアサーションライブラリです。

Expectのコンセプトは[power-assert](https://github.com/power-assert-js/power-assert)や[swift-tesiting](https://github.com/swiftlang/swift-testing)にインスパイアされており、Expression Treeを活用したより良いアサーションを提供します。

## Why Expect?

NUnitなどのテストフレームワークでは、`Assert.That`を用いてアサーションを記述します。以下はNUnitを用いた単体テストの例です。

```cs
// テスト用のrecordを定義
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

このテストは失敗し、以下のようなメッセージが表示されます。

```
Expected: greater than or equal to 18
But was:  16
```

`Assert.That`を用いたアサーションの問題は、利用者に大量の`Is.**`の使い分けを要求することです。多くのテストフレームワークではあらゆるユースケースに対応するために大量のAPIが用意されており、利用する際にはこれらを覚えなければなりません。

また、いくつかのアサーションライブラリでは`foo.Should().Not.Be.Null()`のようなFluent Interfaceを採用していますが、このようなAPIは可読性を低下させるため利用すべきではないと考えています。自然言語に寄せた過度なFluent Interfaceは冗長であり、必ずしも読みやすいとは限りません。テストは自然言語に寄せたものではなく、プログラマフレンドリーな読みやすいコードであるべきです。

Expectでは、提供するAPIは以下の1つだけです。

```cs
using static Expect.Expectation;

[Test]
public void Example()
{
    var person = new Person("Alice", 16);

    // Expectで条件式を渡す
    Expect(() => person.Age >= 18);
}
```

これは以下のようなメッセージを表示します。

```
(person.Age → 16) >= 18
```

Expectは渡された式を解析することで適切なエラーメッセージを表示します。

以下はコレクションのアサートの比較です。

```cs
int[] array = [1, 2, 3, 4, 5];

// NUnit
Assert.That(array, Has.Member(-1));

// Expect
Expect(() => array.Contains(-1));
```

エラーメッセージはそれぞれ以下のようになります。

```
Expected: some item equal to -1
But was:  < 1, 2, 3, 4, 5 >
```

```
(array → [1, 2, 3, 4, 5]).Contains(-1)
```

## インストール

### NuGet packages

Expectを利用するには.NET Standard2.0以上が必要です。パッケージはNuGetから入手できます。

### .NET CLI

```ps1
dotnet add package Expect
```

### Package Manager

```ps1
Install-Package Expect
```

## ライセンス

このライブラリは[MITライセンス](LICENSE)の下に公開されています。
