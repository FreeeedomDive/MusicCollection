using System.Text.Json;
using Microsoft.AspNetCore.Http;
using MusicCollection.Api.Dto.Exceptions;

namespace ApiUtils.Middlewares;

public class ExceptionsMiddleware
{
    private RequestDelegate next;

    public ExceptionsMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (MusicCollectionApiExceptionBase e)
        {
            await WriteExceptionAsync(context, e, e.StatusCode);
            throw;
        }
        catch (Exception e)
        {
            await WriteExceptionAsync(context, e, 500);
            throw;
        }
    }

    private static async Task WriteExceptionAsync(HttpContext context, Exception exception, int statusCode)
    {
        context.Response.StatusCode = statusCode;
        var serializedException = JsonSerializer.Serialize(exception);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(serializedException);
    }
}