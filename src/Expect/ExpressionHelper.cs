using System.Linq.Expressions;

namespace Expect;

internal static class ExpressionHelper
{
    public static string GetOperatorString(BinaryExpression expression)
    {
        return expression.NodeType switch
        {
            ExpressionType.Add or ExpressionType.AddChecked => "+",
            ExpressionType.And => "&",
            ExpressionType.AndAlso => "&&",
            ExpressionType.Divide => "/",
            ExpressionType.Equal => "==",
            ExpressionType.ExclusiveOr => "^",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LeftShift => "<<",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            ExpressionType.Modulo => "%",
            ExpressionType.Multiply or ExpressionType.MultiplyChecked => "*",
            ExpressionType.NotEqual => "!=",
            ExpressionType.Or => "|",
            ExpressionType.OrElse => "||",
            ExpressionType.Power => "^",
            ExpressionType.RightShift => ">>",
            ExpressionType.Subtract or ExpressionType.SubtractChecked => "-",
            ExpressionType.TypeAs => "as",
            ExpressionType.TypeIs => "is",
            ExpressionType.Assign => "=",
            ExpressionType.AddAssign or ExpressionType.AddAssignChecked => "+=",
            ExpressionType.AndAssign => "&=",
            ExpressionType.DivideAssign => "/=",
            ExpressionType.ExclusiveOrAssign => "^=",
            ExpressionType.LeftShiftAssign => "<<=",
            ExpressionType.ModuloAssign => "%=",
            ExpressionType.MultiplyAssign or ExpressionType.MultiplyAssignChecked => "*=",
            ExpressionType.OrAssign => "|=",
            ExpressionType.PowerAssign => "^=",
            ExpressionType.RightShiftAssign => ">>=",
            ExpressionType.SubtractAssign or ExpressionType.SubtractAssignChecked => "-=",
            ExpressionType.Coalesce => "??",
            _ => throw new NotImplementedException(),
        };
    }

    public static string GetTypeName(Type type)
    {
        // if (type == typeof(bool)) return "bool";
        // if (type == typeof(byte)) return "byte";
        // if (type == typeof(sbyte)) return "sbyte";
        // if (type == typeof(short)) return "short";
        // if (type == typeof(ushort)) return "ushort";
        // if (type == typeof(int)) return "int";
        // if (type == typeof(uint)) return "uint";
        // if (type == typeof(long)) return "long";
        // if (type == typeof(ulong)) return "ulong";
        // if (type == typeof(string)) return "string";
        // if (type == typeof(char)) return "char";
        // if (type == typeof(object)) return "object";
        return type.Name;
    }
}