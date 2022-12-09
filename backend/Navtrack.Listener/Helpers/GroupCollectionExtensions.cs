using System;
using System.Text.RegularExpressions;

namespace Navtrack.Listener.Helpers;

public static class GroupCollectionExtensions
{
    public static T Get<T>(this Group group)
    {
        try
        {
            Type underlyingType = Nullable.GetUnderlyingType(typeof(T));

            return (T) Convert.ChangeType(group.Value, underlyingType ?? typeof(T));
        }
        catch (InvalidCastException)
        {
        }
        catch (FormatException)
        {
        }

        return default;
    }
}