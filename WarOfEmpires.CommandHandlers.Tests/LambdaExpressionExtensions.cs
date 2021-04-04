using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace WarOfEmpires.CommandHandlers.Tests {
    public static class LambdaExpressionExtensions {
        public static string GetExpressionText(this LambdaExpression expression) {
            var nameParts = new Stack<string>();
            var part = expression.Body;

            while (part != null) {
                if (part.NodeType == ExpressionType.Call) {
                    var methodExpression = (MethodCallExpression)part;

                    if (!IsSingleArgumentIndexer(methodExpression)) {
                        break;
                    }

                    nameParts.Push(GetIndexerInvocation(methodExpression.Arguments.Single()));

                    part = methodExpression.Object;
                }
                else if (part.NodeType == ExpressionType.ArrayIndex) {
                    var binaryExpression = (BinaryExpression)part;

                    nameParts.Push(GetIndexerInvocation(binaryExpression.Right));

                    part = binaryExpression.Left;
                }
                else if (part.NodeType == ExpressionType.MemberAccess) {
                    var memberExpressionPart = (MemberExpression)part;

                    nameParts.Push("." + memberExpressionPart.Member.Name);

                    part = memberExpressionPart.Expression;
                }
                else if (part.NodeType == ExpressionType.Parameter) {
                    nameParts.Push(string.Empty);

                    part = null;
                }
                else {
                    break;
                }
            }

            if (nameParts.Count > 0 && string.Equals(nameParts.Peek(), ".model", StringComparison.OrdinalIgnoreCase)) {
                nameParts.Pop();
            }

            if (nameParts.Count > 0) {
                return nameParts.Aggregate((left, right) => left + right).TrimStart('.');
            }

            return string.Empty;
        }


        private static string GetIndexerInvocation(Expression expression) {
            var converted = Expression.Convert(expression, typeof(object));
            var fakeParameter = Expression.Parameter(typeof(object), null);
            var lambda = Expression.Lambda<Func<object, object>>(converted, fakeParameter);
            var func = lambda.Compile();

            return "[" + Convert.ToString(func(null), CultureInfo.InvariantCulture) + "]";
        }

        private static bool IsSingleArgumentIndexer(Expression expression) {
            var methodExpression = expression as MethodCallExpression;

            if (methodExpression == null || methodExpression.Arguments.Count != 1) {
                return false;
            }

            return methodExpression.Method
                .DeclaringType
                .GetDefaultMembers()
                .OfType<PropertyInfo>()
                .Any(p => p.GetGetMethod() == methodExpression.Method);
        }
    }
}
