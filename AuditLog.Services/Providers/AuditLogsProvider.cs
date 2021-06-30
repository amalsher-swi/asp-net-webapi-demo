using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AuditLog.Common.Models;
using AuditLog.Data.MySql.Entities;
using AuditLog.Data.MySql.Interfaces.Repositories;
using AuditLog.Services.Extensions;
using AuditLog.Services.Helpers;
using AuditLog.Services.Interfaces.Providers;
using AuditLog.Services.Models;
using AuditLog.Services.Providers.Base;
using AutoMapper;

namespace AuditLog.Services.Providers
{
    public class AuditLogsProvider : BaseProvider<AuditLogModel, int, AuditLogEntity>, IAuditLogsProvider
    {
        private readonly IMapper _mapper;
        private readonly IAuditLogsRepository _auditLogsRepository;

        public AuditLogsProvider(IMapper mapper, IAuditLogsRepository auditLogsRepository)
            : base(mapper, auditLogsRepository)
        {
            _mapper = mapper;
            _auditLogsRepository = auditLogsRepository;
        }

        public async Task<PagedDataResult<AuditLogModel>> FilterByAsync(
            string? filterBy,
            string? filterValue,
            string? sortBy,
            bool sortAsc = true,
            int page = 0,
            int pageSize = 0,
            CancellationToken ct = default)
        {
            Expression<Func<AuditLogEntity, bool>>? filterExp = null;
            if (!string.IsNullOrWhiteSpace(filterBy))
            {
                var propertyInfo = typeof(AuditLogEntity).GetPropertyIgnoringCase(filterBy);
                if (propertyInfo != null)
                {
                    filterExp = ExpressionHelpers.FilterToLambda<AuditLogEntity>(propertyInfo, filterValue!);
                }
            }

            Expression<Func<AuditLogEntity, object>>? sortExp = null;
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var propertyInfo = typeof(AuditLogEntity).GetPropertyIgnoringCase(sortBy);
                if (propertyInfo != null)
                {
                    sortExp = ExpressionHelpers.PropertyToLambda<AuditLogEntity>(propertyInfo.Name);
                }
            }

            var result = await _auditLogsRepository.GetPagedAsync(filterExp, sortExp, sortAsc, page, pageSize, ct);

            return _mapper.Map<PagedDataResult<AuditLogModel>>(result);
        }
    }
}
