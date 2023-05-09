using System.Collections.Generic;
using System.Linq;

namespace Navtrack.Listener.Helpers.New;

public abstract class ChecksumUtil
{
    public static string NMEA(byte[] bytes)
    {
        int checksum = 0;

        for (int i = 1; i < bytes.Length; i++)
        {
            checksum ^= bytes[i];
        }

        return $"{checksum:X2}".ToUpper();
    }

    public static string Xor(byte[] bytes)
    {
        int checksum = bytes.Aggregate(0, (current, t) => current ^ t);

        return $"{checksum:X2}".ToUpper();
    }

    public static byte XorByte(byte[] bytes)
    {
        byte checksum = bytes.Aggregate((byte)0x0, (current, t) => (byte)(current ^ t));

        return checksum;
    }

    public static byte Crc8(byte[] bytes)
    {
        byte crc = 0xFF;
            
        foreach (byte b in bytes)
        {
            crc ^= b;
            for (int i = 0; i < 8; i++)
            {
                crc = (crc & 0x80) != 0 ? (byte) ((crc << 1) ^ 0x31) : (byte) (crc << 1);
            }
        }

        return crc;
    }
        
    public static int Modulo256(IEnumerable<byte> bytes)
    {
        return bytes.Aggregate(0, (current, t) => (current + t) & 0xFF);
    }
}