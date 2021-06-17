namespace AuditLog.Data.MySql.Interfaces
{
    public interface IBaseEntity<out TIdType>
    {
        TIdType Id { get; }
    }
}
