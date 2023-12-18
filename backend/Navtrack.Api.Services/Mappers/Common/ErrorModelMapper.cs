using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.Util;

namespace Navtrack.Api.Services.Mappers.Common;

public static class ErrorModelMapper
{
    public static ErrorModel Map(ApiException exception)
    {
        return new ErrorModel
        {
            Code = exception.Code,
            Message = string.IsNullOrEmpty(exception.Message) ? null : exception.Message,
            Errors = exception.ValidationErrors.GroupBy(x => x.PropertyName.ToCamelCase())
                .ToDictionary(x => x.Key, x => x.Select(y => y.Code).ToArray())
        };
    }

    public static ErrorModel Map(Exception exception)
    {
        return new ErrorModel
        {
            Code = StatusCodes.Status500InternalServerError.ToString(),
            Message = exception.Message
        };
    }

    public static ErrorModel Map(ValidationProblemDetails validationProblemDetails)
    {
        return new ErrorModel
        {
            Code = StatusCodes.Status400BadRequest.ToString(),
            Message = validationProblemDetails.Title,
            Errors = validationProblemDetails.Errors
        };
    }

    public static ErrorModel Map(ProblemDetails problemDetails)
    {
        return new ErrorModel
        {
            Code = problemDetails.Status.ToString(),
            Message = problemDetails.Title
        };
    }
}