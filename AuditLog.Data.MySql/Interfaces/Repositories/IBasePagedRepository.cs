using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AuditLog.Common.Models;

namespace AuditLog.Data.MySql.Interfaces.Repositories
{
    public interface IBasePagedRepository<TEntity, in TIdType> : IBaseRepository<TEntity, TIdType> where TEntity : class, IBaseEntity<TIdType>
    {
        Task<PagedDataResult<TEntity>> GetPagedAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool sortAsc = true,
            int page = 0,
            int pageSize = 0,
            CancellationToken ct = default);
    }
}
