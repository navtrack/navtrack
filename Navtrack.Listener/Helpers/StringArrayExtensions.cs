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
    }
}