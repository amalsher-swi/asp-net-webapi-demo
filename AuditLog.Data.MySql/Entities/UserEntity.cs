using System;
using AuditLog.Data.MySql.Entities.Base;
using LinqToDB.Mapping;

namespace AuditLog.Data.MySql.Entities
{
    [Table(Name = "Users")]
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
    }
}
