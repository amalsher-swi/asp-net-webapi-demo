using System;
using System.Linq;
using System.Reflection;
using LinqToDB.Common;
using LinqToDB.Mapping;
using LinqToDB.Metadata;

namespace AuditLog.Data.MySql.DbContext
{
    public class CustomAttributeReader : IMetadataReader
    {
        private readonly AttributeReader _defaultReader = new();
        
        public TAttribute[] GetAttributes<TAttribute>(Type type, bool inherit = true) where TAttribute : Attribute
        {
            if (typeof(TableAttribute) == typeof(TAttribute))
            {
                var defaultAttributes = _defaultReader.GetAttributes<TAttribute>(type, inherit);
                var defaultAttribute =
                    defaultAttributes.FirstOrDefault(attr => attr.GetType() == typeof(TableAttribute)) as TableAttribute;
                if (defaultAttribute != null)
                {
                    if (string.IsNullOrWhiteSpace(defaultAttribute.Name))
                    {
                        defaultAttribute.Name = type.Name;
                    }

                    return new[] {(defaultAttribute as TAttribute)!};
                }

                var attribute = new TableAttribute
                {
                    Name = type.Name
                };

                return new[] {(attribute as TAttribute)!};
            }

            return Array.Empty<TAttribute>();
        }

        public TAttribute[] GetAttributes<TAttribute>(Type type, MemberInfo memberInfo, bool inherit = true) where TAttribute : Attribute
        {
            if (typeof(ColumnAttribute) == typeof(TAttribute))
            {
                var defaultAttributes = _defaultReader.GetAttributes<TAttribute>(type, memberInfo, inherit);
                var defaultAttribute =
                    defaultAttributes.FirstOrDefault(attr => attr.GetType() == typeof(ColumnAttribute)) as ColumnAttribute;
                if (defaultAttribute != null)
                {
                    if (string.IsNullOrWhiteSpace(defaultAttribute.Name))
                    {
                        defaultAttribute.Name = memberInfo.Name;
                    }

                    return new[] {(defaultAttribute as TAttribute)!};
                }
                
                var attribute = new ColumnAttribute
                {
                    Name = memberInfo.Name
                };
                return new[] {(attribute as TAttribute)!};
            }

            return Array.Empty<TAttribute>();
        }

        public MemberInfo[] GetDynamicColumns(Type type) => Array<MemberInfo>.Empty;
    }
}
