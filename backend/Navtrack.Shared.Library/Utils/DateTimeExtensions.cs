using System;

namespace Navtrack.Shared.Library.Utils;

public static class DateTimeExtensions
{
    /// <summary>
    /// Returns an zero-based index where firstDayOfWeek = 0 and lastDayOfWeek = 6
    /// </summary>
    /// <param name="value"></param>
    /// <param name="firstDayOfWeek"></param>
    /// <returns>int between 0 and 6</returns>
    public static int DayOfWeek(this DateTime value, DayOfWeek firstDayOfWeek)
    {
        int index = 7 + (int)value.DayOfWeek - (int)firstDayOfWeek;
        
        if (index > 6) // week ends at 6, because Enum.DayOfWeek is zero-based
        {
            index -= 7;
        }

        return index;
    }
}