using System;
using System.Threading;
using System.Threading.Tasks;
using AuditLog.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuditLog.API.Middleware
{
    public class CancellationTokenMiddleware
    {
        private const int DefaultTimeoutSeconds = 60;
        
        private readonly RequestDelegate _next;
        private readonly ILogger<CancellationTokenMiddleware> _logger;
        private readonly int _timeoutSeconds;

        public CancellationTokenMiddleware(RequestDelegate next, ILogger<CancellationTokenMiddleware> logger,
            IOptions<CancellationTokenOptions> cancellationTokenOptions)
        {
            _next = next;
            _logger = logger;
            _timeoutSeconds = cancellationTokenOptions.Value.RequestTimeoutSeconds;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var timeout = GetTimeout();

            using var timeoutCts = new CancellationTokenSource(timeout);
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(context.RequestAborted, timeoutCts.Token);

            context.RequestAborted = cts.Token;

            try
            {
                await _next(context);
            }
            catch (OperationCanceledException)
            {
                if (timeoutCts.IsCancellationRequested)
                {
                    _logger.LogInformation($"Request was canceled because the maximum execution time {timeout} was exceeded");
                }
                else if (cts.IsCancellationRequested)
                {
                    _logger.LogDebug("Request was cancelled by user");
                }

                throw;
            }
        }

        private TimeSpan GetTimeout()
        {
#if DEBUG
            return TimeSpan.FromHours(1);
#endif

            return TimeSpan.FromSeconds(_timeoutSeconds == default ? DefaultTimeoutSeconds : _timeoutSeconds);
        }
    }

    public static class CancellationTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseCancellationTokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CancellationTokenMiddleware>();
        }
    }
}
