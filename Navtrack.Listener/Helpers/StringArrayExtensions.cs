using System;

namespace Navtrack.Listener.Helpers
{
    public static class StringArrayExtensions
    {
        public static T Get<T>(this string[] array, int index)
        {
            return array.Length > index ? (T) Convert.ChangeType(array[index], typeof(T)) : default;
        }
    }
}