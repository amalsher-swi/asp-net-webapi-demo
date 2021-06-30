using System;
using System.ComponentModel.DataAnnotations;
using AuditLog.Common.Enums;
using AuditLog.Services.Models.Base;

namespace AuditLog.Services.Models
{
    public class AuditLogModel : BaseModel
    {
        [Required]
        public DateTime Timestamp { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int PartnerId { get; set; }
        [Required]
        public string PartnerName { get; set; } = string.Empty;
        public string? Action { get; set; }
        public string? Entity { get; set; }
        public StatusEnum? Status { get; set; }
        public int? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? TraceId { get; set; }
    }
}
