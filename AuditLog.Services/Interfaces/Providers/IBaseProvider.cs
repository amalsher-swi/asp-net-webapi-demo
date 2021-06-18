using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AuditLog.Services.Interfaces.Providers
{
    public interface IBaseProvider<T, in TIdType> where T : class, IBaseModel<TIdType>
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
        Task<T?> GetByIdAsync(TIdType id, CancellationToken ct = default);
        Task<T?> InsertAsync(T model, CancellationToken ct = default);
        Task<bool> InsertAsync(IReadOnlyCollection<T> models, CancellationToken ct = default);
        Task<bool> UpdateAsync(T model, CancellationToken ct = default);
        Task<bool> DeleteAsync(TIdType id, CancellationToken ct = default);
        Task<bool> DeleteAsync(IReadOnlyCollection<TIdType> ids, CancellationToken ct = default);
    }
}
