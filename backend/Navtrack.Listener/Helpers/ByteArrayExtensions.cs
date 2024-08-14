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

    public static bool ToBoolean(this byte[] array)
    {
        return BitConverter.ToBoolean(array);
    }

    public static sbyte ToSByte1(this byte[] array)
    {
        return Convert.ToSByte(array[0]);
    }

    public static byte ToUByte1(this byte[] array)
    {
        return array[0];
    }

    public static short ToSShort2(this byte[] array)
    {
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(array);
        }

        return BitConverter.ToInt16(array);
    }

    public static ushort ToUShort2(this byte[] array)
    {
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(array);
        }

        return BitConverter.ToUInt16(array);
    }

    public static int ToSInt4(this byte[] array)
    {
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(array);
        }

        return BitConverter.ToInt32(array);
    }

    public static uint ToUInt4(this byte[] array)
    {
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(array);
        }

        return BitConverter.ToUInt32(array);
    }

    public static long ToSLong8(this byte[] array)
    {
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(array);
        }

        return BitConverter.ToInt64(array);
    }

    public static ulong ToULong8(this byte[] array)
    {
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(array);
        }

        return BitConverter.ToUInt64(array);
    }
}