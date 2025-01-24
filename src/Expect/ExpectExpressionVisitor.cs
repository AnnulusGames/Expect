using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Expect;

internal class ExpectExpressionVisitor
{
    public bool CaptureMembers { get; init; } = true;

    bool ShouldCapture(int depth) => CaptureMembers && depth == 1;

    public string Visit(Expression? expression, int depth)
    {
        if (expression == null) return "";

        return expression switch
        {
            BinaryExpression binaryExpression => VisitBinaryExpression(binaryExpression, depth),
            UnaryExpression unaryExpression => VisitUnaryExpression(unaryExpression, depth),
            ConstantExpression constantExpression => VisitConstantExpression(constantExpression, depth),
            MemberExpression memberExpression => VisitMemberExpression(memberExpression, depth),
            MethodCallExpression methodCallExpression => VisitMethodCallExpression(methodCallExpression, depth),
            NewExpression newExpression => VisitNewExpression(newExpression, depth),
            MemberInitExpression memberInitExpression => VisitMemberInitExpression(memberInitExpression, depth),
            LambdaExpression lambdaExpression => VisitLambdaExpression(lambdaExpression, depth),
            ConditionalExpression conditionalExpression => VisitConditionalExpression(conditionalExpression, depth),
            NewArrayExpression newArrayExpression => VisitNewArrayExpression(newArrayExpression, depth),
            _ => expression.ToString()
        };
    }

    string VisitNewArrayExpression(NewArrayExpression expression, int depth)
    {
        var newExpr = $"new {ExpressionHelper.GetTypeName(expression.Type.GetElementType()!)}";

        string message = "";
        switch (expression.NodeType)
        {
            case ExpressionType.NewArrayBounds:
                var length = Visit(expression.Expressions[0], depth + 1);
                message = $"{newExpr}[{length}]";
                break;
            case ExpressionType.NewArrayInit:
                message = $"{newExpr}[]{{ {string.Join(", ", expression.Expressions.Select(x => Visit(x, depth + 1)))} }}";
                break;
        }

        return message;
    }

    string VisitConditionalExpression(ConditionalExpression expression, int depth)
    {
        var test = Visit(expression.Test, depth + 1);
        var ifTrue = Visit(expression.IfTrue, depth + 1);
        var ifFalse = Visit(expression.IfFalse, depth + 1);

        var message = $"{test} ? {ifTrue} : {ifFalse}";

        if (ShouldCapture(depth))
        {
            var value = Expression.Lambda(expression).Compile().DynamicInvoke();
            message = $"({message} → {ConvertValueToString(value)})";
        }

        return message;
    }

    string VisitLambdaExpression(LambdaExpression expression, int depth)
    {
        var parameters = string.Join(", ", expression.Parameters.Select(x => x.ToString()));
        if (expression.Parameters.Count != 1) parameters = $"({parameters})";

        return $"{parameters} => {Visit(expression.Body, depth + 1)}";
    }

    string VisitNewExpression(NewExpression expression, int depth)
    {
        var message = $"new {ExpressionHelper.GetTypeName(expression.Type)}()";
        if (ShouldCapture(depth))
        {
            var value = Expression.Lambda(expression).Compile().DynamicInvoke();
            message = $"({message} → {ConvertValueToString(value)})";
        }
        return message;
    }

    string VisitMemberInitExpression(MemberInitExpression expression, int depth)
    {
        var message = $"new {ExpressionHelper.GetTypeName(expression.Type)}()";
        if (ShouldCapture(depth))
        {
            var value = Expression.Lambda(expression).Compile().DynamicInvoke();
            message = $"({message} → {ConvertValueToString(value)})";
        }
        return message;
    }

    string VisitMethodCallExpression(MethodCallExpression expression, int depth)
    {
        var parent = Visit(expression.Object, depth + 1);
        var path = string.IsNullOrEmpty(parent) ? expression.Method.Name : $"{parent}.{expression.Method.Name}";

        string message;
        if (expression.Method.GetCustomAttribute<ExtensionAttribute>() != null)
        {
            var arg0 = Visit(expression.Arguments[0], depth + 1);
            var parameters = string.Join(", ", expression.Arguments.Skip(1).Select(x => Visit(x, depth + 1)));
            message = $"{arg0}.{path}({parameters})";
        }
        else
        {
            message = $"{path}({string.Join(", ", expression.Arguments.Select(x => Visit(x, depth + 1)))})";
        }

        if (ShouldCapture(depth))
        {
            var value = Expression.Lambda(expression).Compile().DynamicInvoke();
            message = $"({message} → {ConvertValueToString(value)})";
        }

        return message;

    }

    string VisitMemberExpression(MemberExpression expression, int depth)
    {
        var parent = Visit(expression.Expression, depth + 1);
        var path = string.IsNullOrEmpty(parent) ? expression.Member.Name : $"{parent}.{expression.Member.Name}";

        if (ShouldCapture(depth))
        {
            var value = Expression.Lambda(expression).Compile().DynamicInvoke();
            path = $"({path} → {ConvertValueToString(value)})";
        }

        return path;
    }

    string VisitUnaryExpression(UnaryExpression expression, int depth)
    {
        var operand = Visit(expression.Operand, depth + 1);

        var message = expression.NodeType switch
        {
            ExpressionType.UnaryPlus => $"+{operand}",
            ExpressionType.Negate or ExpressionType.NegateChecked => $"-{operand}",
            ExpressionType.Not => $"!{operand}",
            ExpressionType.TypeAs => $"as {operand}",
            ExpressionType.ArrayLength => $"{operand}.Length",
            ExpressionType.Convert or ExpressionType.ConvertChecked => $"({ExpressionHelper.GetTypeName(expression.Type)}){operand}",
            _ => throw new NotImplementedException()
        };

        if (ShouldCapture(depth))
        {
            var value = Expression.Lambda(expression).Compile().DynamicInvoke();
            message = $"({message} → {ConvertValueToString(value)})";
        }

        return message;
    }

    string VisitBinaryExpression(BinaryExpression expression, int depth)
    {
        string message;
        switch (expression.NodeType)
        {
            case ExpressionType.ArrayIndex:
                {
                    var left = Visit(expression.Left, depth + 1);
                    var right = Visit(expression.Right, depth + 1);
                    message = $"{left}[{right}]";
                    break;
                }
            default:
                {
                    var left = Visit(expression.Left, depth + 1);
                    var right = Visit(expression.Right, depth + 1);
                    var operatorStr = ExpressionHelper.GetOperatorString(expression);
                    message = $"{left} {operatorStr} {right}";
                    break;
                }
        }

        if (ShouldCapture(depth))
        {
            var value = Expression.Lambda(expression).Compile().DynamicInvoke();
            message = $"(({message}) → {ConvertValueToString(value)})";
        }

        return message;
    }

    string VisitConstantExpression(ConstantExpression expression, int depth)
    {
        return ConvertValueToString(expression.Value);
    }

    string ConvertValueToString(object? value)
    {
        var message = value == null ? "null" : value.ToString()!;

        // compiler generated
        if (message.Contains("<>")) return "";

        var type = value?.GetType();
        if (type == typeof(string)) message = @$"""{message}""";
        else if (type == typeof(char)) message = $"'{message}'";
        else if (value is IEnumerable enumerable) message = $"[{string.Join(", ", AsEnumerable(enumerable).Select(ConvertValueToString))}]";

        return message;
    }

    static IEnumerable<object?> AsEnumerable(IEnumerable array)
    {
        foreach (var item in array) yield return item;
    }
}