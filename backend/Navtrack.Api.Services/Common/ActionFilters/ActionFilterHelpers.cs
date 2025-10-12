using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

namespace Navtrack.Api.Services.Common.ActionFilters;

public static class ActionFilterHelpers
{
    public static string? GetId(HttpContext httpContext, string key)
    {
        if (httpContext.Request.Query.TryGetValue(key, out StringValues queryValue))
        {
            return queryValue.ToString();
        }

        if (httpContext.GetRouteData().Values.TryGetValue(key, out object? routeValue))
        {
            return routeValue?.ToString();
        }

        return null;
    }
}