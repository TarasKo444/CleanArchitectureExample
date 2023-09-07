using CleanArchitecture.Common.Exceptions;
using FluentValidation;

namespace CleanArchitecture.Web.Middlewares;

public class CustomExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException e)
        {
            await HandleValidationExceptionAsync(context, e);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int code = 500;
        string message = string.Empty;
        
        switch (exception)
        {
            case UserFriendlyException e:
                code = e.Status;
                message = e.Message;
                break;
        }

        context.Response.StatusCode = code;
        await context.Response.WriteAsJsonAsync(new { Status = code, Message = message });
    }
    private static async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        const int code = 400;
        context.Response.StatusCode = code;
        await context.Response.WriteAsJsonAsync(new { Status = code, Errors = exception.Errors.Select(e => e.ErrorMessage) });
    }
}

public static class CustomExtensionHandlerExtension
{
    public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CustomExceptionMiddleware>();
    }
}