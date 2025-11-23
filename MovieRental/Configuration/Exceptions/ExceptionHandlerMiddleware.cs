using Microsoft.AspNetCore.Mvc;

namespace MovieRental.Configuration.Exceptions
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }
        private static readonly Dictionary<Type, (int Status, string Title)> ErrorMap =
            new()
            {
                    { typeof(PaymentFailedException), (StatusCodes.Status422UnprocessableEntity, "Payment Failed") },
                    { typeof(PaymentProviderNotFoundException), (StatusCodes.Status400BadRequest, "Payment Provider Not Found") },
                    { typeof(ArgumentException), (StatusCodes.Status400BadRequest, "Bad Request") }
            };

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var error = MapToProblemDetails(ex, context);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = error.Status ?? StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(error);
            }
        }

        private ProblemDetails MapToProblemDetails(Exception ex, HttpContext context)
        {
            var mapping = ErrorMap
                .FirstOrDefault(m => m.Key.IsAssignableFrom(ex.GetType()));

            if (mapping.Key != null)
            {
                return new ProblemDetails
                {
                    Status = mapping.Value.Status,
                    Title = mapping.Value.Title,
                    Detail = ex.Message,
                };
            }

            return new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Unexpected Error",
                Detail = "An unexpected error has occurred.",
            };
        }
    }
}
