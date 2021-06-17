using LinqToDB.Mapping;

namespace AuditLog.Data.MySql.DbContext
{
    public static class LinqToDbSettings
    {
        public static void SetUp()
        {
            LinqToDB.Common.Configuration.ContinueOnCapturedContext = false;
            MappingSchema.Default.AddMetadataReader(new CustomAttributeReader());
        }
    }
}
