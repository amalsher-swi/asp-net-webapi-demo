using System.Threading;
using System.Threading.Tasks;
using AuditLog.Common.Models;
using AuditLog.Services.Models;

namespace AuditLog.Services.Interfaces.Providers
{
    public interface IAuditLogsProvider : IBaseProvider<AuditLogModel, int>
    {
        Task<PagedDataResult<AuditLogModel>> FilterByAsync(
            string? filterBy,
            string? filterValue,
            string? sortBy,
            bool sortAsc = true,
            int page = 0,
            int pageSize = 0,
            CancellationToken ct = default);
    }
}
