using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuditLog.Services.Models;

namespace AuditLog.Services.Interfaces.Providers
{
    public interface IUsersProvider : IBaseProvider<User, int>
    {
        Task<IEnumerable<User>> SearchAsync(string searchText, CancellationToken ct = default);
    }
}
