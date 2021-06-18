using System;

namespace AuditLog.Data.MySql.Interfaces
{
    public interface IBaseEntity<TIdType>
    {
        TIdType Id { get; set; }
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
    }
}
