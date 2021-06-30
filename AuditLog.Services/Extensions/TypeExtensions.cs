using System;
using System.Reflection;

namespace AuditLog.Services.Extensions
{
    public static class TypeExtensions
    {
        public static PropertyInfo? GetPropertyIgnoringCase(this Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            return property;
        }
    }
}
