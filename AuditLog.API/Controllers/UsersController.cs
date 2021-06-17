using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuditLog.Data.MySql.Entities;
using AuditLog.Data.MySql.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AuditLog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpGet]
        public Task<IEnumerable<UserEntity>> Get(CancellationToken ct)
        {
            return _usersRepository.FilterByAsync(null, ct);
        }

        [HttpGet("{id:int}")]
        public Task<UserEntity?> GetById(int id, CancellationToken ct)
        {
            return _usersRepository.GetByIdAsync(id, ct);
        }
    }
}
