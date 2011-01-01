﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentAssertions.Common
{
    internal static class Extensions
    {
        public static PropertyInfo GetPropertyInfo<T>(this Expression<Func<T, object>> expression)
        {
            if (ReferenceEquals(expression, null))
            {
                throw new NullReferenceException("Expected a property expression, but found <null>.");
            }

            PropertyInfo propertyInfo = AttemptToGetPropertyInfoFromCastExpression(expression);
            if (propertyInfo == null)
            {
                propertyInfo = AttemptToGetPropertyInfoFromPropertyExpression(expression);
            }

            if (propertyInfo == null)
            {
                throw new ArgumentException("Cannot use <" + expression.Body + "> when a property expression is expected.");
            }

            return propertyInfo;
        }

        private static PropertyInfo AttemptToGetPropertyInfoFromPropertyExpression<T>(Expression<Func<T, object>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                return (PropertyInfo)memberExpression.Member;
            }

            return null;
        }

        private static PropertyInfo AttemptToGetPropertyInfoFromCastExpression<T>(Expression<Func<T, object>> expression)
        {
            var castExpression = expression.Body as UnaryExpression;
            if (castExpression != null)
            {
                return (PropertyInfo)((MemberExpression)castExpression.Operand).Member;
            }

            return null;
        }

        public static int IndexOfFirstMismatch(this string value, string expected)
        {
            for (int index = 0; index < value.Length; index++)
            {
                if ((index >= expected.Length) || (value[index] != expected[index]))
                {
                    return index;
                }
            }

            return -1;
        }

        public static string Mismatch(this string value, int index)
        {
            int length = Math.Min(value.Length - index, 3);

            return string.Format("'{0}' (index {1})", value.Substring(index, length), index);
        }

        public static bool IsEqualTo(this object actual, object expected)
        {
            if (ReferenceEquals(actual, null) && ReferenceEquals(expected, null))
            {
                return true;
            }

            if (ReferenceEquals(actual, null))
            {
                return false;
            }

            return actual.Equals(expected);
        }
    }
}
