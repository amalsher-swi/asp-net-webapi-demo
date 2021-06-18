using System;
using AuditLog.Data.MySql.Interfaces;
using LinqToDB.Mapping;

namespace AuditLog.Data.MySql.Entities.Base
{
    public class BaseEntity : IBaseEntity<int>
    {
        [PrimaryKey, Identity]
        public int Id { get; set; }
        public DateTime Updated { get; set; } 
        public DateTime Created { get; set; }
    }
}
