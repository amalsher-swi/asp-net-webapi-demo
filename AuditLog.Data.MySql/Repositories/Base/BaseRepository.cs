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
using LinqToDB.Data;

namespace AuditLog.Data.MySql.Repositories.Base
{
    public abstract class BaseRepository<TEntity, TIdType> : IBaseRepository<TEntity, TIdType> where TEntity : class, IBaseEntity<TIdType>
    {
        private readonly AppDataConnection _dbConnection;

        protected BaseRepository(AppDataConnection dbConnection)
        {
            _dbConnection = dbConnection;
            Table = dbConnection.GetTable<TEntity>();
        }

        protected ITable<TEntity> Table { get; }

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

        public async Task<TEntity?> InsertAsync(TEntity entity, CancellationToken ct = default)
        {
            entity.Updated = DateTime.UtcNow;
            entity.Created = DateTime.UtcNow;
            
            if (typeof(TIdType) == typeof(int))
            {
                var intId = await _dbConnection.InsertWithInt32IdentityAsync(entity, token: ct);
                if (intId is TIdType id)
                {
                    entity.Id = id;
                    return entity;
                }
            }
            else
            {
                var objectId = await _dbConnection.InsertWithIdentityAsync(entity, token: ct);
                if (objectId is TIdType id)
                {
                    entity.Id = id;
                    return entity;
                }
            }

            return default;
        }

        public async Task<bool> InsertAsync(IReadOnlyCollection<TEntity> entities, CancellationToken ct = default)
        {
            foreach (var entity in entities)
            {
                entity.Updated = DateTime.UtcNow;
                entity.Created = DateTime.UtcNow;
            }

            var result = await Table.BulkCopyAsync(entities, ct);
            // TODO: process the result of operation
            return result.RowsCopied == entities.Count;
        }

        public async Task<bool> UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            entity.Updated = DateTime.UtcNow;

            var result = await _dbConnection.UpdateAsync(entity, token: ct);
            return result == 1;
        }

        public async Task<bool> DeleteAsync(TIdType id, CancellationToken ct = default)
        {
            var result = await Table.DeleteAsync(e => e.Id!.Equals(id), ct);
            return result == 1;
        }

        public async Task<bool> DeleteAsync(IReadOnlyCollection<TIdType> ids, CancellationToken ct = default)
        {
            var result = await Table.DeleteAsync(e => ids.Contains(e.Id), ct);
            return result == ids.Count;
        }
    }
}
