using AuditLog.Data.MySql.Entities;

namespace AuditLog.Data.MySql.Interfaces.Repositories
{
    public interface IAuditLogsRepository : IBaseRepository<AuditLogEntity, int>
    {
    }
}
