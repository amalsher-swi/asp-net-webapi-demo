using AuditLog.Data.MySql.Entities;
using AuditLog.Data.MySql.Interfaces.Repositories;
using AuditLog.Services.Interfaces.Providers;
using AuditLog.Services.Models;
using AuditLog.Services.Providers.Base;
using AutoMapper;

namespace AuditLog.Services.Providers
{
    public class AuditLogsProvider : BaseProvider<AuditLogModel, int, AuditLogEntity>, IAuditLogsProvider
    {
        public AuditLogsProvider(IMapper mapper, IAuditLogsRepository auditLogsRepository)
            : base(mapper, auditLogsRepository)
        {
        }
    }
}
