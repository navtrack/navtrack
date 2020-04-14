using System.Collections.Generic;
using System.Linq;

namespace Navtrack.Listener.Extensions
{
    public static class Crc16
    {
        private const int HashSize = 16;
        private const ulong Init = 0xFFFF;
        private const ulong Mask = ulong.MaxValue >> (64 - 16);
        private const ulong Poly = 0x1021;
    
        // ReSharper disable once IdentifierTypo
        public static ulong Ccitt(IEnumerable<byte> bytes)
        {
            ulong crc = Init;
            ulong[] table = Enumerable.Range(0, 256).Select(CreateTableEntry).ToArray();

            int toRight = HashSize - 8;
            toRight = toRight < 0 ? 0 : toRight;
            
            foreach (byte t in bytes)
            {
                crc = table[((crc >> toRight) ^ t) & 0xFF] ^ (crc << 8);
                crc &= Mask;
            }

            return crc;
        }

        private static ulong CreateTableEntry(int index)
        {
            ulong r = (ulong) index;
            r <<= HashSize - 8;

            const ulong lastBit = 1ul << (HashSize - 1);

            for (int i = 0; i < 8; i++)
            {
                if ((r & lastBit) != 0)
                {
                    r = (r << 1) ^ Poly;
                }
                else
                {
                    r <<= 1;
                }
            }

            return r & Mask;
        }
    }
}