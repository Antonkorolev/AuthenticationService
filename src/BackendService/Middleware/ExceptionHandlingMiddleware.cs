using BackendService.BusinessLogic.Exceptions;

namespace BackendService.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (UserNotFoundException e)
        {
            _logger.LogInformation($"User not found, exception was throw: {e}");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("User not found").ConfigureAwait(false);
        }
        catch (IOException e)
        {
            _logger.LogError(e.Message);
        }
    }
}