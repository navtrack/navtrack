using System;
using static System.String;

namespace Navtrack.Listener.Helpers;

public static class StringArrayExtensions
{
    public static T Get<T>(this string[] array, int index)
    {
        try
        {
            if (array.Length > index)
            {
                Type underlyingType = Nullable.GetUnderlyingType(typeof(T));

                return (T) Convert.ChangeType(array[index], underlyingType ?? typeof(T));
            }
        }
        catch (InvalidCastException)
        {
        }
        catch (FormatException)
        {
        }

        return default;
    }

    public static double? GetDouble(this string[] array, int index, int? round = null)
    {
        if (array.Length > index && double.TryParse(array[index], out double value))
        {
            return round.HasValue ? Math.Round(value, round.Value) : value;
        }

        return default;
    }
        
    public static string StringJoin(this string[] array)
    {
        return Join(Empty, array);
    }
}