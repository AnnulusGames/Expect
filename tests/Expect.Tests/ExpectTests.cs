
namespace Expect.Tests;

using static Expect.Expectation;

public class ExpectTests
{
    [Test]
    public void Test_Expect_Equals()
    {
        Assert.That(() =>
        {
            var a = 0;
            Expectation.Expect(() => a == 1);
        }, Throws.TypeOf<ExpectationFailedException>().With.Message.EqualTo("Expectation failed: \n(a → 0) == 1"));
    }

    [Test]
    public void Test_Expect_NotEquals()
    {
        Assert.That(() =>
        {
            var a = 0;
            Expectation.Expect(() => a != 0);
        }, Throws.TypeOf<ExpectationFailedException>().With.Message.EqualTo("Expectation failed: \n(a → 0) != 0"));
    }

    [Test]
    public void Test_Expect_And()
    {
        Assert.That(() =>
        {
            var a = 10;
            Expectation.Expect(() => 0 < a && a < 10);
        }, Throws.TypeOf<ExpectationFailedException>().With.Message.EqualTo("Expectation failed: \n((0 < a) → True) && ((a < 10) → False)"));
    }

    [Test]
    public void Test_Expect_Null()
    {
        Assert.That(() =>
        {
            var a = new object();
            Expectation.Expect(() => a == null);
        }, Throws.TypeOf<ExpectationFailedException>().With.Message.EqualTo("Expectation failed: \n(a → System.Object) == null"));

        var a = 10;
        var b = 10;
        var c = 10;
        Expect(() => a == 10 && b == 10 && c == 20);
    }
}

record Person(string Name, int Age);

record TestModel
{
    public int Foo { get; set; }
    public string Bar { get; set; } = "";
}
