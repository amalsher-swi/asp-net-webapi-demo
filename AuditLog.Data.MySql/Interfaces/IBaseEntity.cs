namespace AuditLog.Data.MySql.Interfaces
{
    public interface IBaseEntity<TIdType>
    {
        TIdType Id { get; set; }
    }
}
