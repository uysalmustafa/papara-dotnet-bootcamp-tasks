namespace ProductsAPI.Middleware
{
    using System.Net;
    using System.Net.Mime;
    using System.Text.Json;

    // Middleware to handle global errors in the application
    public class GlobalErrorHandlingMiddleware
    {
        // Field to hold the next middleware in the pipeline
        private readonly RequestDelegate _next;

        // Constructor to initialize the middleware with the next middleware delegate
        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Call the next middleware in the pipeline
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                // Handle exceptions and create a response
                var response = context.Response;
                response.ContentType = MediaTypeNames.Application.Json;

                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errorResponse = new
                {
                    message = ex.Message,
                    statusCode = response.StatusCode,
                    title = "Error"
                };

                // Serialize the error response to JSON
                var errorJson = JsonSerializer.Serialize(errorResponse);
                await response.WriteAsync(errorJson);
            }
        }
    }
}
