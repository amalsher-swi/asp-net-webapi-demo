using AuditLog.Data.MySql.Entities;

namespace AuditLog.Data.MySql.Interfaces.Repositories
{
    public interface IUsersRepository : IBaseRepository<UserEntity, int>
    {
    }
}
