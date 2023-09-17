using SimpleWebApp.Api.CustomExceptions;
using System.Net;

namespace SimpleWebApp.Api.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (BadRequestExcepion e)
            {
                await HandleExceptionAsync(context, new ValidationFailure[] { new ValidationFailure(String.Empty, e.Message) }, HttpStatusCode.BadRequest);
            }
            catch (LockedException e)
            {
                await HandleExceptionAsync(context, new ValidationFailure[] { new ValidationFailure(String.Empty, e.Message) }, 403);
            }
            catch (NoFoundException e)
            {
                await HandleExceptionAsync(context, new ValidationFailure[] { new ValidationFailure(String.Empty, e.Message) }, HttpStatusCode.NotFound);
            }
            catch (ValidationFailureException e)
            {
                await HandleExceptionAsync(context, e.Errors, 420);
            }
            catch (FluentValidation.ValidationException ex)
            {
                var errors = ex.Errors
                    .Select(x => new ValidationFailure(x.PropertyName, x.ErrorMessage));

                await HandleExceptionAsync(context, errors, HttpStatusCode.BadRequest);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, IEnumerable<ValidationFailure> errors, HttpStatusCode statusCode)
        {
            return HandleExceptionAsync(context, errors, (int)statusCode);
        }

        private Task HandleExceptionAsync(HttpContext context, IEnumerable<ValidationFailure> errors, int statusCode)
        {
            context.Response.StatusCode = statusCode;

            var errorsList = errors.ToArray();
            if (errorsList.Length > 0)
            {
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync(new ErrorDetails(errorsList).SerializeToJson());
            }

            return Task.CompletedTask;
        }
    }
}
