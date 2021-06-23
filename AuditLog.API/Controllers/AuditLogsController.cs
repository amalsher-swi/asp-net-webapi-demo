using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuditLog.Services.Interfaces.Providers;
using AuditLog.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuditLog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogsProvider _auditLogsProvider;

        public AuditLogsController(IAuditLogsProvider auditLogsProvider)
        {
            _auditLogsProvider = auditLogsProvider;
        }

        [HttpGet]
        public Task<IEnumerable<AuditLogModel>> Get(CancellationToken ct)
        {
            return _auditLogsProvider.GetAllAsync(ct);
        }

        [HttpGet("{id:int}")]
        public Task<AuditLogModel?> GetById(int id, CancellationToken ct)
        {
            return _auditLogsProvider.GetByIdAsync(id, ct);
        }

        [HttpPost]
        public Task<AuditLogModel?> Insert(AuditLogModel model, CancellationToken ct)
        {
            return _auditLogsProvider.InsertAsync(model, ct);
        }

        [HttpPost("many")]
        public Task<bool> Insert(IReadOnlyCollection<AuditLogModel> models, CancellationToken ct)
        {
            return _auditLogsProvider.InsertAsync(models, ct);
        }

        [HttpPut]
        public Task<bool> Update(AuditLogModel model, CancellationToken ct)
        {
            return _auditLogsProvider.UpdateAsync(model, ct);
        }

        [HttpDelete]
        public Task<bool> Delete(int id, CancellationToken ct)
        {
            return _auditLogsProvider.DeleteAsync(id, ct);
        }

        [HttpDelete("many")]
        public Task<bool> Delete(IReadOnlyCollection<int> ids, CancellationToken ct)
        {
            return _auditLogsProvider.DeleteAsync(ids, ct);
        }
    }
}
