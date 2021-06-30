using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AuditLog.Common.Models;
using AuditLog.Data.MySql.DbContext;
using AuditLog.Data.MySql.Interfaces;
using AuditLog.Data.MySql.Interfaces.Repositories;
using LinqToDB;

namespace AuditLog.Data.MySql.Repositories.Base
{
    public abstract class BasePagedRepository<TEntity, TIdType>
        : BaseRepository<TEntity, TIdType>, IBasePagedRepository<TEntity, TIdType> where TEntity : class, IBaseEntity<TIdType>
    {
        protected BasePagedRepository(AppDataConnection dbConnection) : base(dbConnection)
        {
        }

        public async Task<PagedDataResult<TEntity>> GetPagedAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool sortAsc = true,
            int page = 0,
            int pageSize = 0,
            CancellationToken ct = default)
        {
            IQueryable<TEntity> allEntities = Table;

            if (filter is not null)
            {
                allEntities = allEntities.Where(filter);
            }

            if (orderBy is not null)
            {
                allEntities = sortAsc ? allEntities.OrderBy(orderBy) : allEntities.OrderByDescending(orderBy);
            }

            IQueryable<TEntity> pagedEntities = allEntities;

            if (pageSize != 0)
            {
                pagedEntities = pagedEntities.Skip(page * pageSize).Take(pageSize);
            }

            var result = new PagedDataResult<TEntity>
            {
                Data = await pagedEntities.ToListAsync(ct),
                Total = await allEntities.CountAsync(ct)
            };

            return result;
        }
    }
}
