using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Navtrack.Api.Model.Errors;

namespace Navtrack.Api.Services.IdentityServer;

public static class GrantValidationResultMapper
{
    public static GrantValidationResult Map(ApiError apiError, Dictionary<string, object>? extraData = null)
    {
        extraData ??= new Dictionary<string, object>();

        extraData["code"] = apiError.Code;

        return new GrantValidationResult(TokenRequestErrors.InvalidRequest, apiError.Message, extraData);
    }
}