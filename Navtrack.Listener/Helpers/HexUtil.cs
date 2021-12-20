using System;
using System.Collections.Generic;
using System.Linq;
using static System.String;

namespace Navtrack.Listener.Helpers;

public static class HexUtil
{
    public static string[] ConvertByteArrayToHexStringArray(IEnumerable<byte> byteArray)
    {
        return byteArray.Select(x => x.ToString("X2")).ToArray();
    }

    public static string ConvertHexStringArrayToHexString(string[] hex)
    {
        return Join(Empty, hex);
    }

    public static byte[] ConvertHexStringToByteArray(string hexString)
    {
        return ConvertHexStringArrayToByteArray(ConvertHexStringToHexStringArray(hexString))
            .ToArray();
    }

    public static string[] ConvertHexStringToHexStringArray(string hexString)
    {
        return Enumerable.Range(0, hexString.Length)
            .Where(x => x % 2 == 0)
            .Select(x => hexString.Substring(x, 2))
            .ToArray();
    }

    public static byte[] ConvertHexStringArrayToByteArray(string[] hexArray)
    {
        return hexArray.Select(x => Convert.ToByte(x, 16)).ToArray();
    }

    public static string ConvertStringToHexString(string data)
    {
        return ConvertHexStringArrayToHexString(
            ConvertByteArrayToHexStringArray(StringUtil.ConvertStringToByteArray(data)));
    }
}