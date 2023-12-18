using System;
using System.Collections.Generic;
using System.Net;
using Navtrack.Api.Model.Errors;

namespace Navtrack.Api.Services.Exceptions;

public class ApiException(HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, string? message = null)
    : Exception(message)
{
    public readonly string Code;
    public readonly List<ValidationError> ValidationErrors = [];
    public readonly HttpStatusCode HttpStatusCode = httpStatusCode;

    public ApiException(ApiError apiError, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : this(httpStatusCode, apiError.Message)
    {
        Code = apiError.Code;
    }
}