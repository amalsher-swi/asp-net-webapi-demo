using System;

namespace AuditLog.Services.Interfaces
{
    public interface IBaseModel<TIdType>
    {
        TIdType Id { get; set; }
        DateTime? Created { get; set; }
        DateTime? Updated { get; set; }
    }
}
