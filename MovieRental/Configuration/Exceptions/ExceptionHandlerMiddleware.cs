using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MovieRental.Configuration.Exceptions
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _log;

        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlerMiddleware> log)
        {
            _next = next;
            _log = log;
        }

        private static Dictionary<Type, ProblemDetails> ErrorMap = new Dictionary<Type, ProblemDetails>()
        {
            {typeof(PaymentFailedException), new ProblemDetails{
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Payment Failed"} },
            {typeof(PaymentProviderNotFoundException), new ProblemDetails{
                Status = StatusCodes.Status400BadRequest,
                Title = "Payment Provider Not Found"} },
            {typeof(ArgumentException), new ProblemDetails{
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request"} },
        };

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                var errorDetails = new ProblemDetails();

                if (ErrorMap.TryGetValue(ex.GetType(), out var mappedError))
                {
                    errorDetails = mappedError;
                    errorDetails.Detail = ex.Message;
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    errorDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "Unexpected Error",
                        //Detail = "An unexpected error has ocurred"
                    };
                }

                errorDetails.Detail = ex.Message;
                context.Response.StatusCode = errorDetails.Status!.Value;
                await context.Response.WriteAsJsonAsync(errorDetails);
            }
        }
    }
}
