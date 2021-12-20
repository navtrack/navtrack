namespace Navtrack.Api.Services;

public static class Util
{
    public static string ToCamelCase(this string value)
    {
        return char.ToLowerInvariant(value[0]) + value[1..];
    }
}