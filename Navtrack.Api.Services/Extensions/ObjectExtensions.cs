using System.Net;
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
        
    public static void ThrowApiExceptionIfNull(this object @object, HttpStatusCode httpStatusCode)
    {
        if (@object == null)
        {
            throw new ApiException(httpStatusCode);
        }
    }
}