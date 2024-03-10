using Fitter_API.Controllers.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Fitter_API.Controllers
{
    public class HttpExceptionFilter : IActionFilter, IOrderedFilter
    {
        private readonly ILogger<HttpExceptionFilter> logger;

        public int Order => int.MaxValue - 10;

        public HttpExceptionFilter(ILogger<HttpExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // do nothing
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                if (context.Exception is NotFoundException NotFound)
                {
                    context.Result = new ObjectResult(NotFound.Message)
                    {
                        StatusCode = NotFound.StatusCode,
                        Value = NotFound.Message
                    };
                    context.ExceptionHandled = true;
                }
                else
                {
                    context.Result = new ObjectResult(context.Exception.Message)
                    {
                        StatusCode = 500
                    };
                    context.ExceptionHandled = true;
                    logger.LogError(context.Exception.Message);
                }
            }
        }
    }
}
