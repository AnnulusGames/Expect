namespace Expect;

public sealed class ExpectationFailedException(string? message, string? expression) : Exception($"Expectation failed: {message}\n{expression}")
{
}