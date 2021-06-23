using AuditLog.Services.Models;

namespace AuditLog.Services.Interfaces.Providers
{
    public interface IAuditLogsProvider : IBaseProvider<AuditLogModel, int>
    {
    }
}
