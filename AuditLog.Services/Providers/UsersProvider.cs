using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuditLog.Data.MySql.Entities;
using AuditLog.Data.MySql.Interfaces.Repositories;
using AuditLog.Services.Interfaces.Providers;
using AuditLog.Services.Models;
using AuditLog.Services.Providers.Base;
using AutoMapper;

namespace AuditLog.Services.Providers
{
    public class UsersProvider : BaseProvider<User, int, UserEntity>, IUsersProvider
    {
        private readonly IMapper _mapper;
        private readonly IUsersRepository _usersRepository;

        public UsersProvider(IMapper mapper, IUsersRepository usersRepository) : base(mapper, usersRepository)
        {
            _mapper = mapper;
            _usersRepository = usersRepository;
        }

        public async Task<IEnumerable<User>> SearchAsync(string searchText, CancellationToken ct = default)
        {
#pragma warning disable CA1307
            var entities = await _usersRepository.FilterByAsync(u =>
                    u.FirstName.Contains(searchText)
                    || (u.LastName != null && u.LastName.Contains(searchText))
                    || u.Email.Contains(searchText),
                ct);
#pragma warning restore CA1307
            return _mapper.Map<IEnumerable<User>>(entities);
        }
    }
}
