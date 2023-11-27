using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Navtrack.Api.Services.ActionFilters;

public static class ActionFilterHelpers
{
    public static string? GetId(string key, ActionContext context)
    {
        if (context.RouteData.Values.TryGetValue(key, out object? routeValue))
        {
            return routeValue?.ToString();
        }

        if (context.HttpContext.Request.Query.TryGetValue(key, out StringValues queryValue))
        {
            return queryValue;
        }

        return null;
    }
}