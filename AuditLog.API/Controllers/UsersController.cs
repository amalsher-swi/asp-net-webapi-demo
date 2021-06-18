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
    public class UsersController : ControllerBase
    {
        private readonly IUsersProvider _usersProvider;

        public UsersController(IUsersProvider usersProvider)
        {
            _usersProvider = usersProvider;
        }

        [HttpGet]
        public Task<IEnumerable<User>> Get(CancellationToken ct)
        {
            return _usersProvider.GetAllAsync(ct);
        }

        [HttpGet("{id:int}")]
        public Task<User?> GetById(int id, CancellationToken ct)
        {
            return _usersProvider.GetByIdAsync(id, ct);
        }

        [HttpGet("search")]
        public Task<IEnumerable<User>> Search(string searchText, CancellationToken ct)
        {
            return _usersProvider.SearchAsync(searchText, ct);
        }

        [HttpPost]
        public Task<User?> Insert(User model, CancellationToken ct)
        {
            return _usersProvider.InsertAsync(model, ct);
        }

        [HttpPost("many")]
        public Task<bool> Insert(IReadOnlyCollection<User> models, CancellationToken ct)
        {
            return _usersProvider.InsertAsync(models, ct);
        }

        [HttpPut]
        public Task<bool> Update(User model, CancellationToken ct)
        {
            return _usersProvider.UpdateAsync(model, ct);
        }

        [HttpDelete]
        public Task<bool> Delete(int id, CancellationToken ct)
        {
            return _usersProvider.DeleteAsync(id, ct);
        }

        [HttpDelete("many")]
        public Task<bool> Delete(IReadOnlyCollection<int> ids, CancellationToken ct)
        {
            return _usersProvider.DeleteAsync(ids, ct);
        }
    }
}
