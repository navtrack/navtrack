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

        public T Get<T>()
        {
            object value = null;
            
            if (typeof(T) == typeof(short))
            {
                value = BitConverter.ToInt16(Get(2));
            }

            else if (typeof(T) == typeof(int))
            {
                value = BitConverter.ToInt32(Get(4));
            }
            
            else if (typeof(T) == typeof(long))
            {
                value = BitConverter.ToInt32(Get(8));
            }
            else if (typeof(T) == typeof(double))
            {
                value = BitConverter.ToDouble(Get(8));
            }
            else if (typeof(T) == typeof(decimal))
            {
                value = BitConverter.ToDouble(Get(16));
            }
            else if (typeof(T) == typeof(float))
            {
                value = BitConverter.ToSingle(Get(4));
            }

            return (T)value;
        }
        
        public T GetLe<T>()
        {
            object value = null;
            
            if (typeof(T) == typeof(short))
            {
                value = BitConverter.ToInt16(Get(2).Reverse().ToArray());
            }

            else if (typeof(T) == typeof(int))
            {
                value = BitConverter.ToInt32(Get(4).Reverse().ToArray());
            }
            
            else if (typeof(T) == typeof(long))
            {
                value = BitConverter.ToInt32(Get(8).Reverse().ToArray());
            }
            else if (typeof(T) == typeof(double))
            {
                value = BitConverter.ToDouble(Get(8).Reverse().ToArray());
            }
            else if (typeof(T) == typeof(decimal))
            {
                value = BitConverter.ToDouble(Get(16).Reverse().ToArray());
            }
            else if (typeof(T) == typeof(float))
            {
                value = BitConverter.ToSingle(Get(4).Reverse().ToArray());
            }

            return (T)value;
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
            if (i > 0)
            {
                index += i;
            }

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