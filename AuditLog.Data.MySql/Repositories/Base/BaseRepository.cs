using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AuditLog.Data.MySql.DbContext;
using AuditLog.Data.MySql.Interfaces;
using AuditLog.Data.MySql.Interfaces.Repositories;
using LinqToDB;

namespace AuditLog.Data.MySql.Repositories.Base
{
    public abstract class BaseRepository<TEntity, TIdType> : IBaseRepository<TEntity, TIdType> where TEntity : class, IBaseEntity<TIdType>
    {
        protected BaseRepository(AppDataConnection dbConnection)
        {
            Table = dbConnection.GetTable<TEntity>();
        }
        
        protected IQueryable<TEntity> Table { get; }

        public async Task<IEnumerable<TEntity>> FilterByAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken ct = default)
        {
            if (filter is null)
            {
                return await Table.ToListAsync(ct);
            }

            return await Table.Where(filter).ToListAsync(ct);
        }

        public Task<TEntity?> GetByIdAsync(TIdType id, CancellationToken ct = default)
        {
            return Table.FirstOrDefaultAsync(entity => entity.Id!.Equals(id), ct);
        }
    }
}
