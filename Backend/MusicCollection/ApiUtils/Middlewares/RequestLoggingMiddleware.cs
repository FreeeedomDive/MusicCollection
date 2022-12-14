using Microsoft.AspNetCore.Http;
using MusicCollection.Common.Loggers.NLog;

using ILogger = MusicCollection.Common.Loggers.ILogger;

namespace ApiUtils.Middlewares;

public class RequestLoggingMiddleware
{
    public RequestLoggingMiddleware(RequestDelegate next)
    {
        this.next = next;
        logger = NLogLogger.Build("RequestLog");
        errorLogger = NLogLogger.Build("ErrorLog");
    }
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch(Exception e)
        {
            errorLogger.Error(e, "Unhandled exception in api method {method}", context.Request?.Method);
        }
        finally
        {
            logger.Info(
                "Request {method} {url} with response status code {statusCode}",
                context.Request?.Method,
                context.Request?.Path.Value,
                context.Response?.StatusCode);
        }
    }

    private readonly ILogger logger;
    private readonly ILogger errorLogger;
    private readonly RequestDelegate next;
}