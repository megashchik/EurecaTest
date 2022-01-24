using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Model.Exceptions;
using System.Diagnostics;

namespace EvricaApi.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        Dictionary<Type, Action<ExceptionContext>> ExceptionHandlers { get; } = new Dictionary<Type, Action<ExceptionContext>>();

        public ApiExceptionFilterAttribute()
        {
            ExceptionHandlers.Add(typeof(EntityAlreadyExistsException), HandleAlreadyExistsException);
            ExceptionHandlers.Add(typeof(EntityNotFoundException), HandleNotFoundException);
            ExceptionHandlers.Add(typeof(InvalidEntityException), HandleInvalidEntityException);
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
            
            base.OnException(context);
        }

        void HandleException(ExceptionContext context)
        {
            Trace.WriteLine("Trace: " + context.Exception);

            Type type = context.Exception.GetType();

            if (ExceptionHandlers.ContainsKey(type))
            {
                ExceptionHandlers[type](context);
                return;
            }

            if(!context.ModelState.IsValid)
            {
                InvalidModelStateHundler(context);
            }


            HandleUnknownException(context);
        }


        void InvalidModelStateHundler(ExceptionContext context)
        {
            ProblemDetails details = new ProblemDetails()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Model state was invalid",
                Detail = context.Exception.Message
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        void HandleNotFoundException(ExceptionContext context)
        {
            ProblemDetails details = new ProblemDetails()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = context.Exception.Message
            };

            context.Result = new NotFoundObjectResult(details);

            context.ExceptionHandled = true;
        }

        void HandleAlreadyExistsException(ExceptionContext context)
        {
            ProblemDetails details = new ProblemDetails()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
                Title = "The specified resource already exists.",
                Detail = context.Exception.Message
            };

            context.Result = new ConflictObjectResult(details);

            context.ExceptionHandled = true;
        }

        void HandleInvalidEntityException(ExceptionContext context)
        {
            ProblemDetails details = new ProblemDetails()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Server received invalid data",
                Detail = context.Exception.Message
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            ProblemDetails details = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}
