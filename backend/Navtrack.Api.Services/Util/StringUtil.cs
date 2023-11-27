namespace Navtrack.Api.Services.Util;

public static class StringUtil
{
    public static string ToCamelCase(this string value)
    {
        return char.ToLowerInvariant(value[0]) + value[1..];
    }
}