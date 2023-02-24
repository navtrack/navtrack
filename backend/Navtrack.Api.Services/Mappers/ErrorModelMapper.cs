using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Exceptions;

namespace Navtrack.Api.Services.Mappers;

public static class ErrorModelMapper
{
    public static ErrorModel Map(ApiException exception)
    {
        return new ErrorModel
        {
            Code = exception.Code,
            Message = string.IsNullOrEmpty(exception.Message) ? null : exception.Message,
            ValidationErrors = exception.HasValidationErrors
                ? exception.ValidationErrors.Select(x => new ValidationErrorModel
                {
                    Code = x.Code,
                    Message = x.Message,
                    PropertyName = x.PropertyName.ToCamelCase()
                }).ToList()
                : null
        };
    }

    public static ErrorModel Map(ModelStateDictionary contextModelState)
    {
        return new ErrorModel
        {
            Message = "Validation failed.",
            ValidationErrors = contextModelState.Select(entry => new
                {
                    Key = entry.Key,
                    Errors = entry.Value?.Errors.Select(modelError => new ValidationErrorModel
                    {
                        PropertyName = entry.Key.ToCamelCase(),
                        Message = modelError.ErrorMessage
                    }).ToList()
                })
                .SelectMany(x => x.Errors)
                .ToList()
        };
    }
}