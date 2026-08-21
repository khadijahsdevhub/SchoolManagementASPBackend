using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;




namespace SchoolManagementASPBackend.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occurred");


            var (statusCode, title, detail) = exception switch
            {

                InvalidStudentIdException => (StatusCodes.Status400BadRequest, "Bad Request", exception.Message),
                StudentNotFoundException => (StatusCodes.Status404NotFound, "Student Not Found", exception.Message),
                DuplicateStudentException => (StatusCodes.Status409Conflict, "Duplicate Student", exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.", "An unexpected error occurred while processing your request.")
            };



            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/problem+json";
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            };

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }



    }
}
