namespace Navtrack.Shared.Library.Extensions;

public static class StringExtensions
{
    public static string ToCamelCase(this string value)
    {
        return char.ToLowerInvariant(value[0]) + value[1..];
    }
}