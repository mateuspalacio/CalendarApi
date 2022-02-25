using Calendar.Domain.Exceptions;

namespace Calendar.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try 
            {
                await _next(context);
            }
            catch (Exception ex) {
                if (ex is ErrorResponseException error)
                {
                    var response = new Error() { };
                    response.Message = error.ErrorResponse.Message;
                    response.StatusCode = error.ErrorResponse.StatusCode;
                    context.Response.StatusCode = error.ErrorResponse.StatusCode;
                    await context.Response.WriteAsJsonAsync(response);
                }
            }
            
        }
    }
    public class Error
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
