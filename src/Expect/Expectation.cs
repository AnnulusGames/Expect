using System.Linq.Expressions;
using System.Diagnostics;

namespace Expect;

public static class Expectation
{
    [StackTraceHidden]
    public static void Expect(Expression<Func<bool>> expression, string? message = null)
    {
        if (!expression.Compile().Invoke())
        {
            var visitor = new ExpectExpressionVisitor();
            throw new ExpectationFailedException(message, visitor.Visit(expression.Body, 0));
        }
    }
}
