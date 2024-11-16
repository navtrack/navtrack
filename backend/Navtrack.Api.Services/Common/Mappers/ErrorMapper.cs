using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Shared.Library.Extensions;

namespace Navtrack.Api.Services.Common.Mappers;

public static class ErrorMapper
{
    public static Error Map(ApiException exception)
    {
        Error error = new Error
        {
            Code = exception.Code,
            Message = string.IsNullOrEmpty(exception.Message) ? null : exception.Message,
            Errors = exception.ValidationErrors.GroupBy(x => x.PropertyName.ToCamelCase())
                .ToDictionary(x => x.Key, x => x.Select(y => y.Code).ToArray())
        };
        
        error.Errors = error.Errors.Any() ? error.Errors : null;
        
        
        return error;
    }

    public static Error Map(Exception exception)
    {
        return new Error
        {
            Code = StatusCodes.Status500InternalServerError.ToString(),
            Message = exception.Message
        };
    }

    public static Error Map(ValidationProblemDetails validationProblemDetails)
    {
        return new Error
        {
            Code = StatusCodes.Status400BadRequest.ToString(),
            Message = validationProblemDetails.Title,
            Errors = validationProblemDetails.Errors
        };
    }

    public static Error Map(ProblemDetails problemDetails)
    {
        return new Error
        {
            Code = problemDetails.Status.ToString(),
            Message = problemDetails.Title
        };
    }
}