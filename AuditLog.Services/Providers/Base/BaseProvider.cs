using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuditLog.Data.MySql.Interfaces;
using AuditLog.Data.MySql.Interfaces.Repositories;
using AuditLog.Services.Interfaces;
using AuditLog.Services.Interfaces.Providers;
using AutoMapper;

namespace AuditLog.Services.Providers.Base
{
    public abstract class BaseProvider<T, TIdType, TEntity> : IBaseProvider<T, TIdType>
        where T : class, IBaseModel<TIdType>
        where TEntity : class, IBaseEntity<TIdType>
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<TEntity, TIdType> _repository;

        protected BaseProvider(IMapper mapper, IBaseRepository<TEntity, TIdType> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _repository.FilterByAsync(null, ct);
            return _mapper.Map<IEnumerable<T>>(entities);
        }

        public async Task<T?> GetByIdAsync(TIdType id, CancellationToken ct = default)
        {
            var entity = await _repository.GetByIdAsync(id, ct);
            return _mapper.Map<T>(entity);
        }

        public async Task<T?> InsertAsync(T model, CancellationToken ct = default)
        {
            var entity = _mapper.Map<TEntity>(model);
            entity = await _repository.InsertAsync(entity, ct);

            return _mapper.Map<T>(entity);
        }

        public Task<bool> InsertAsync(IReadOnlyCollection<T> models, CancellationToken ct = default)
        {
            var entities = _mapper.Map<IReadOnlyCollection<TEntity>>(models);
            return _repository.InsertAsync(entities, ct);
        }

        public Task<bool> UpdateAsync(T model, CancellationToken ct = default)
        {
            var entity = _mapper.Map<TEntity>(model);
            return _repository.UpdateAsync(entity, ct);
        }

        public Task<bool> DeleteAsync(TIdType id, CancellationToken ct = default)
        {
            return _repository.DeleteAsync(id, ct);
        }

        public Task<bool> DeleteAsync(IReadOnlyCollection<TIdType> ids, CancellationToken ct = default)
        {
            return _repository.DeleteAsync(ids, ct);
        }
    }
}
