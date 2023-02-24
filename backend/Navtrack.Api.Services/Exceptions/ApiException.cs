using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Navtrack.Api.Model.Errors;

namespace Navtrack.Api.Services.Exceptions;

public class ApiException : Exception
{
    public readonly string Code;
    public readonly List<ValidationError> ValidationErrors = new();
    public readonly HttpStatusCode HttpStatusCode;

    public ApiException(HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, string? message = null) :
        base(message)
    {
        HttpStatusCode = httpStatusCode;
    }

    public ApiException(ApiError apiError, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : base(
        apiError.Message)
    {
        Code = apiError.Code;
        HttpStatusCode = httpStatusCode;
    }

    public ApiException AddValidationError(string propertyName, string errorMessage)
    {
        ValidationErrors.Add(new ValidationError
        {
            PropertyName = propertyName,
            Message = errorMessage
        });

        return this;
    }

    public ApiException AddValidationError(string propertyName, ApiError apiError)
    {
        ValidationErrors.Add(new ValidationError
        {
            PropertyName = propertyName,
            Message = apiError.Message,
            Code = apiError.Code
        });

        return this;
    }

    public void ThrowIfInvalid()
    {
        if (HasValidationErrors)
        {
            throw this;
        }
    }

    public bool HasValidationErrors => ValidationErrors.Any();
}