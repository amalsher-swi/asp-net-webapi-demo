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
    }
}
