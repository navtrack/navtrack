using System;
using System.Linq;

namespace Navtrack.Listener.Helpers;

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

    public static short ToInt16(this byte[] array)
    {
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(array);
        }

        return BitConverter.ToInt16(array);
    }
        
    public static int ToInt32(this byte[] array)
    {
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(array);
        }

        return BitConverter.ToInt32(array);
    }
        
    public static long ToInt64(this byte[] array)
    {
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(array);
        }

        return BitConverter.ToInt64(array);
    }
}