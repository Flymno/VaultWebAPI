using Microsoft.AspNetCore.Diagnostics;
using VaultWebAPI.DTOs;

namespace VaultWebAPI.Exceptions
{
    public class VaultExceptionHandler : IExceptionHandler
    {
        private readonly ILogger _logger;
        
        public VaultExceptionHandler(ILogger<VaultExceptionHandler> logger)
        {
            _logger = logger; 
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,  Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Vault Error Encountered: {Message}", exception.Message);

            int statusCode;
            string errorTitle;
            string errorDescription;

            switch (exception)
            {
                case UnauthorizedVaultException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    errorTitle = "Unauthorized";
                    errorDescription = exception.Message;
                    break;

                case NotFoundVaultException:
                    statusCode = StatusCodes.Status404NotFound;
                    errorTitle = "Not Found";
                    errorDescription = exception.Message;
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    errorTitle = "Internal Server Error";
                    errorDescription = "An unexpected error occurred on the server";
                    break;
            }

            ErrorResponseDTO response = new ErrorResponseDTO(statusCode, errorTitle, errorDescription);
            
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}
