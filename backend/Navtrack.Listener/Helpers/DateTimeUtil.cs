using System;
using System.Globalization;

namespace Navtrack.Listener.Helpers;

public static class DateTimeUtil
{
    public static DateTime New(string year, string month, string day, string hour, string minute, string second,
        string millisecond = null, bool add2000Year = true)
    {
        DateTime dateTime = new(add2000Year ? Convert.ToInt32(year) + 2000 : Convert.ToInt32(year),
            Convert.ToInt32(month),
            Convert.ToInt32(day),
            Convert.ToInt32(hour),
            Convert.ToInt32(minute),
            Convert.ToInt32(second),
            string.IsNullOrEmpty(millisecond) ? 0 : Convert.ToInt32(millisecond));
        
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }

    public static DateTime NewFromHex(string yearHex, string monthHex, string dayHex, string hourHex,
        string minuteHex, string secondHex,
        string millisecondHex = null, bool add2000Year = true)
    {
        int year = int.Parse(yearHex, NumberStyles.HexNumber);
        int month = int.Parse(monthHex, NumberStyles.HexNumber);
        int day = int.Parse(dayHex, NumberStyles.HexNumber);
        int hour = int.Parse(hourHex, NumberStyles.HexNumber);
        int minute = int.Parse(minuteHex, NumberStyles.HexNumber);
        int second = int.Parse(secondHex, NumberStyles.HexNumber);
        int millisecond = string.IsNullOrEmpty(millisecondHex)
            ? 0
            : int.Parse(millisecondHex, NumberStyles.HexNumber);

        DateTime dateTime = new(add2000Year ? year + 2000 : year, month, day, hour, minute, second, millisecond);

        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
}