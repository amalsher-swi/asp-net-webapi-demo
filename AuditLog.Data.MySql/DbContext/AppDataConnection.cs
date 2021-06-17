using AuditLog.Data.MySql.Entities;
using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;

namespace AuditLog.Data.MySql.DbContext
{
    public class AppDataConnection : DataConnection
    {
        public AppDataConnection(LinqToDbConnectionOptions<AppDataConnection> options) : base(options)
        {
        }

        public ITable<UserEntity> Users => GetTable<UserEntity>();
    }
}
