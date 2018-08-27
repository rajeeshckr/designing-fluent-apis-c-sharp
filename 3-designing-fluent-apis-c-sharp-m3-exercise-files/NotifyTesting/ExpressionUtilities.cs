using System;
using System.Linq.Expressions;

namespace NotifyTesting
{
    internal static class ExpressionUtilities
    {
        public static string GetPropertyName<T, TProp>(Expression<Func<T, TProp>> propertyExpression)
        {
            var body = propertyExpression.Body as MemberExpression;

            if (body == null)
                throw new ArgumentException("Expression must be a simple property access" +
                     " of the form \"x => x.PropertyName\".");

            return body.Member.Name;
        }
    }
}
