using AuditLog.Data.MySql.DbContext;
using AuditLog.Data.MySql.Entities;
using AuditLog.Data.MySql.Interfaces.Repositories;
using AuditLog.Data.MySql.Repositories.Base;

namespace AuditLog.Data.MySql.Repositories
{
    public class UsersRepository : BaseRepository<UserEntity, int>, IUsersRepository
    {
        public UsersRepository(AppDataConnection dbConnection) : base(dbConnection)
        {
        }
    }
}
