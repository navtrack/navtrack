using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Navtrack.Api.Model.Errors;

namespace Navtrack.Api.Services.Common.Exceptions;

public static class ApiExceptionUtil
{
    public static void Return404IfTrue(this bool boolean)
    {
        if (boolean)
        {
            throw new ApiException(HttpStatusCode.NotFound);
        }
    }

    public static void ReturnIfNull([NotNull]this object? @object, HttpStatusCode httpStatusCode)
    {
        if (@object == null)
        {
            throw new ApiException(httpStatusCode);
        }
    }
    
    public static void Return404IfNull([NotNull]this object? @object)
    {
        ReturnIfNull(@object, HttpStatusCode.NotFound);
    }

    public static void ReturnValidationErrorIfNull([NotNull]this object? @object, string fieldName, ApiError apiError)
    {
        if (@object == null)
        {
            throw new ValidationApiException().AddValidationError(fieldName, apiError);
        }
    }   
   
    public static void ThrowValidationApiIfNull([NotNull]this object? @object, Action<ValidationApiException> func)
    {
        if (@object == null)
        {
            ValidationApiException exception = new();
            
            func(exception);

            throw exception;
        }
    }
    
    public static void ThrowApiExceptionIfNull([NotNull]this object? @object, HttpStatusCode httpStatusCode,
        string? message = null)
    {
        if (@object == null)
        {
            throw new ApiException(httpStatusCode, message);
        }
    }

    public static void ThrowApiExceptionIfTrue(this bool boolean, ApiError apiError)
    {
        if (boolean)
        {
            throw new ApiException(apiError); 
        }
    }
}