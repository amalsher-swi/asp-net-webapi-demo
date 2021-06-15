using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace AuditLog.API.Helpers
{
    public class ActivityEnricher : ILogEventEnricher
    {
        private const string SpanId = "SpanId";
        private const string TraceId = "TraceId";
        private const string ParentId = "ParentId";
        
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var activity = Activity.Current;

            if (activity is not null)
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty(SpanId, new ScalarValue(activity.GetSpanId())));
                logEvent.AddPropertyIfAbsent(new LogEventProperty(TraceId, new ScalarValue(activity.GetTraceId())));
                logEvent.AddPropertyIfAbsent(new LogEventProperty(ParentId, new ScalarValue(activity.GetParentId())));
            }
        }
    }

    internal static class ActivityExtensions
    {
        public static string GetSpanId(this Activity activity) => activity.SpanId.ToHexString();
        public static string GetTraceId(this Activity activity) => activity.TraceId.ToHexString();
        public static string GetParentId(this Activity activity) => activity.ParentSpanId.ToHexString();
    }
}
