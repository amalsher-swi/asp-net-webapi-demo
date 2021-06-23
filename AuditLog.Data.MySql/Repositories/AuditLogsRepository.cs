using AuditLog.Data.MySql.DbContext;
using AuditLog.Data.MySql.Entities;
using AuditLog.Data.MySql.Interfaces.Repositories;
using AuditLog.Data.MySql.Repositories.Base;

namespace AuditLog.Data.MySql.Repositories
{
    public class AuditLogsRepository : BaseRepository<AuditLogEntity, int>, IAuditLogsRepository
    {
        public AuditLogsRepository(AppDataConnection dbConnection) : base(dbConnection)
        {
        }
    }
}
