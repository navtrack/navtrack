using System;

namespace Navtrack.Listener.Helpers
{
    public static class StringArrayExtensions
    {
        public static T Get<T>(this string[] array, int index)
        {
            try
            {
                if (array.Length > index)
                {
                    return (T) Convert.ChangeType(array[index], typeof(T));
                }
            }
            catch (InvalidCastException)
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
    }
}