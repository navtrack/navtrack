using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Shared.Library.Extensions;

namespace Navtrack.Api.Services.Common.Mappers;

public static class ProblemDetailsMapper
{
    public static ValidationProblemDetails Map(ValidationApiException exception)
    {
        ValidationProblemDetails problemDetails = new(exception.ValidationErrors
            .GroupBy(x => x.PropertyName.ToCamelCase())
            .ToDictionary(x => x.Key, x => x.Select(y => y.Code).ToArray())
        )
        {
            Status = (int)exception.HttpStatusCode,
            Title = exception.Code
        };

        return problemDetails;
    }

    public static ProblemDetails Map(ApiException exception)
    {
        ProblemDetails problemDetails = new()
        {
            Status = (int)exception.HttpStatusCode,
            Title = exception.Code
        };

        return problemDetails;
    }

    public static ProblemDetails Map(ProblemDetails problemDetails)
    {
        return problemDetails;
    }

    public static ValidationProblemDetails Map(ModelStateDictionary modelStateDictionary)
    {
        ValidationProblemDetails problemDetails = new(modelStateDictionary)
        {
            Title = ApiErrorCodes.Validation_Generic.Code
        };
        
        return problemDetails;
    }
}