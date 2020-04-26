using System;
using System.Linq;

namespace Navtrack.Listener.Server
{
    public class ByteReader
    {
        private byte[] bytes;
        private int index;

        public ByteReader(byte[] bytes)
        {
            this.bytes = bytes;
            index = 0;
        }

        public byte GetOne()
        {
            return GetNext(1, false).First();
        }

        public byte[] Get(int i)
        {
            return GetNext(i, false);
        }

        private byte[] GetNext(int i, bool skipOne)
        {
            int endIndex = index + i;
            
            byte[] sub = bytes[index..endIndex];

            index = skipOne ? index + i + 1 : index + i;

            return sub;
        } 

        public ByteReader Skip(int i)
        {
            index += i;

            return this;
        }

        public byte[] GetUntil(byte b, int? extra = null)
        {
            int newIndex = Array.IndexOf(bytes.Skip(index).ToArray(), b);

            if (extra.HasValue)
            {
                newIndex += extra.Value;
            }
            
            return GetNext(newIndex, true);
        }

        public void Reset()
        {
            index = 0;
        }
    }
}