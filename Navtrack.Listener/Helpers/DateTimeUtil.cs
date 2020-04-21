using System;

namespace Navtrack.Listener.Helpers
{
    public static class DateTimeUtil
    {
        public static DateTime New(string year, string month, string day, string hour, string minute, string second,
            string millisecond = null)
        {
            return new DateTime(Convert.ToInt32(year) + 2000,
                Convert.ToInt32(month),
                Convert.ToInt32(day),
                Convert.ToInt32(hour),
                Convert.ToInt32(minute),
                Convert.ToInt32(second),
                string.IsNullOrEmpty(millisecond) ? 0 : Convert.ToInt32(millisecond));
        }
    }
}