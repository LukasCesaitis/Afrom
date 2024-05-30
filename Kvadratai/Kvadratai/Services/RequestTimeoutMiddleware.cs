namespace Kvadratai.Services
{
    public class RequestTimeoutMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimeoutMiddleware> _logger;
        private readonly TimeSpan _timeout;

        public RequestTimeoutMiddleware(RequestDelegate next, ILogger<RequestTimeoutMiddleware> logger, TimeSpan timeout)
        {
            _next = next;
            _logger = logger;
            _timeout = timeout;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var cts = new CancellationTokenSource(_timeout))
            {
                try
                {
                    context.RequestAborted = cts.Token;
                    await _next(context);
                }
                catch (OperationCanceledException) when (cts.IsCancellationRequested)
                {
                    _logger.LogWarning("Request timed out.");
                    context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                    await context.Response.WriteAsync("Request timed out.");
                }
            }
        }
    }
}
