using System.Collections.Generic;

namespace AuditLog.Common.Models
{
    public class PagedDataResult<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int Total { get; set; }
    }
}
