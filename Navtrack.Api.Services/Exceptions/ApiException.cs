using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Navtrack.Api.Services.Exceptions;

public class ApiException : Exception
{
    public ApiException(HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest,
        string message = null) : base(message)
    {
        this.httpStatusCode = httpStatusCode;
        ValidationErrors = new List<ValidationError>();
    }

    public string Code { get; set; }
    public List<ValidationError> ValidationErrors { get; }

    public readonly HttpStatusCode httpStatusCode;

    public bool HasErrors => ValidationErrors != null && ValidationErrors.Any();

    public ApiException AddValidationError(string propertyName, string errorMessage)
    {
        ValidationErrors.Add(new ValidationError
        {
            PropertyName = propertyName,
            Message = errorMessage
        });

        return this;
    }

    public void ThrowIfInvalid()
    {
        if (HasErrors)
        {
            throw this;
        }
    }
}