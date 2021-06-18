using System;
using AuditLog.Services.Interfaces;

namespace AuditLog.Services.Models.Base
{
    public abstract class BaseModel : IBaseModel<int>
    {
        public int Id { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
