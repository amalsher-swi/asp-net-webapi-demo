using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace AuditLog.Data.MySql.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity, in TIdType> where TEntity : class, IBaseEntity<TIdType>
    {
        Task<IEnumerable<TEntity>> FilterByAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken ct = default);
        Task<TEntity?> GetByIdAsync(TIdType id, CancellationToken ct = default);
        Task<TEntity?> InsertAsync(TEntity entity, CancellationToken ct = default);
        Task<bool> InsertAsync(IReadOnlyCollection<TEntity> entities, CancellationToken ct = default);
        Task<bool> UpdateAsync(TEntity entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(TIdType id, CancellationToken ct = default);
        Task<bool> DeleteAsync(IReadOnlyCollection<TIdType> ids, CancellationToken ct = default);
    }
}
