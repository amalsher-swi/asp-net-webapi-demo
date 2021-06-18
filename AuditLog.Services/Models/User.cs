using System;
using System.ComponentModel.DataAnnotations;
using AuditLog.Services.Models.Base;

namespace AuditLog.Services.Models
{
    public class User : BaseModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        public string? LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime Birthday { get; set; }
    }
}
