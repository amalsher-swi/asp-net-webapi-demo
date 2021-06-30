using System;
using System.Linq.Expressions;
using System.Reflection;

namespace AuditLog.Services.Helpers
{
    public static class ExpressionHelpers
    {
        private const string ErrorMessage = "{0} value is incorrect";

        public static Expression<Func<T, object>> PropertyToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

        public static Expression<Func<T, bool>> FilterToLambda<T>(PropertyInfo propertyInfo, string filterValue)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyInfo.Name);

            ConstantExpression valueExpression;

            if (propertyInfo.PropertyType == typeof(bool))
            {
                if (bool.TryParse(filterValue, out var value))
                {
                    valueExpression = Expression.Constant(value, propertyInfo.PropertyType);
                }
                else
                {
                    throw new ArgumentException(string.Format(ErrorMessage, nameof(Boolean)));
                }
            }
            else if (propertyInfo.PropertyType == typeof(int))
            {
                if (int.TryParse(filterValue, out var value))
                {
                    valueExpression = Expression.Constant(value, propertyInfo.PropertyType);
                }
                else
                {
                    throw new ArgumentException(string.Format(ErrorMessage, nameof(Int32)));
                }
            }
            else if (propertyInfo.PropertyType == typeof(long))
            {
                if (long.TryParse(filterValue, out var value))
                {
                    valueExpression = Expression.Constant(value, propertyInfo.PropertyType);
                }
                else
                {
                    throw new ArgumentException(string.Format(ErrorMessage, nameof(Int64)));
                }
            }
            else if (propertyInfo.PropertyType == typeof(DateTime))
            {
                if (DateTime.TryParse(filterValue, out var value))
                {
                    valueExpression = Expression.Constant(value, propertyInfo.PropertyType);
                }
                else
                {
                    throw new ArgumentException(string.Format(ErrorMessage, nameof(DateTime)));
                }
            }
            else if (propertyInfo.PropertyType.IsEnum)
            {
                if (int.TryParse(filterValue, out var enumValue))
                {
                    valueExpression = Expression.Constant(Enum.ToObject(propertyInfo.PropertyType, enumValue));
                }
                else
                {
                    if (Enum.TryParse(propertyInfo.PropertyType, filterValue, true, out var value))
                    {
                        valueExpression = Expression.Constant(Enum.ToObject(propertyInfo.PropertyType, (int)value!));    
                    }
                    else
                    {
                        throw new ArgumentException(string.Format(ErrorMessage, nameof(Enum)));
                    }
                }
            }
            else
            {
                valueExpression = Expression.Constant(filterValue.ToUpper());

                var selector = Expression.Lambda<Func<T, object>>(property, parameter);
                Expression containsValueExpression;

                if (propertyInfo.PropertyType == typeof(string))
                {
                    var toUpperMethod = typeof(string).GetMethod("ToUpper",
                        BindingFlags.Instance | BindingFlags.Public, null, Array.Empty<Type>(), null);
                    var lowercaseExpression = Expression.Call(selector.Body, toUpperMethod!);
                    containsValueExpression = Expression.Call(lowercaseExpression, "Contains", null, valueExpression);
                }
                else
                {
                    containsValueExpression = Expression.Call(selector.Body, "Contains", null, valueExpression);
                }

                return Expression.Lambda<Func<T, bool>>(containsValueExpression, parameter);
            }

            var equalsExp = Expression.Equal(property, valueExpression);
            return Expression.Lambda<Func<T, bool>>(equalsExp, parameter);
        }
    }
}
