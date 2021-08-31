using System;
using AuditLog.Common.Enums;
using AuditLog.Data.MySql.Entities.Base;
using LinqToDB.Mapping;

namespace AuditLog.Data.MySql.Entities
{
    [Table(Name = "AuditLogs")]
    public class AuditLogEntity : BaseEntity
    {
        public DateTime Timestamp { get; set; }
        public int UserId { get; set; }
        public int PartnerId { get; set; }
        public string PartnerName { get; set; } = string.Empty;
        public string? Action { get; set; }
        public string? Entity { get; set; }
        public AuditLogStatus? Status { get; set; }
        public int? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? TraceId { get; set; }
    }
}
