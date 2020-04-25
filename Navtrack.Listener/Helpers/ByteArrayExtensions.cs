using System.Linq;

namespace Navtrack.Listener.Helpers
{
    public static class ByteArrayExtensions
    {
        public static int? GetStartIndex(this byte[] array, byte[] match)
        {
            for (int i = 0; i < array.Length - match.Length; i++)
            {
                int endIndex = i + match.Length;

                if (array[i..endIndex].IsEqual(match))
                {
                    return i;
                }
            }

            return null;
        }
        
        public static bool IsEqual(this byte[] array1, byte[] array2)
        {
            return !array1.Where((t, i) => t != array2[i]).Any();
        }
    }
}