using System.Net;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Exceptions;

namespace Navtrack.Api.Services.Extensions;

public static class ObjectExtensions
{
    public static void Throw404IfNull(this object @object)
    {
        if (@object == null)
        {
            throw new ApiException(HttpStatusCode.NotFound);
        }
    }

    public static void ThrowApiExceptionIfNull(this object @object, HttpStatusCode httpStatusCode,
        string? message = null)
    {
        if (@object == null)
        {
            throw new ApiException(httpStatusCode, message);
        }
    }

    public static void ThrowApiExceptionIfTrue(this bool boolean, HttpStatusCode httpStatusCode, string? message = null)
    {
        if (boolean)
        {
            throw new ApiException(httpStatusCode, message);
        }
    }

    public static void ThrowApiExceptionIfFalse(this bool boolean, HttpStatusCode httpStatusCode,
        string? message = null)
    {
        if (!boolean)
        {
            throw new ApiException(httpStatusCode, message);
        }
    }

    public static void ThrowApiExceptionIfNullOrEmpty(this string? value, ApiError apiError)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ApiException(apiError);
        }
    }
}