using Lotto.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lotto.Api.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public ApiExceptionFilterAttribute()
        {
        }

        public override void OnException(ExceptionContext context)
        {
            var errorModel = new ErrorsResponse { Errors = new List<ErrorResponse>() };
            var statusCode = 500;

            if (context.Exception is ApiException apiException)
            {
                // _logger.LogInformation(apiException, apiException.Message);
                errorModel.Message = apiException.Message;
                statusCode = apiException.StatusCode;
            }
            else
            {
                // _logger.LogError(context.Exception, "unexpected error");
                errorModel.Message = context.Exception.InnerException?.Message ?? context.Exception.Message;
            }

            context.Result = new ObjectResult(errorModel)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
