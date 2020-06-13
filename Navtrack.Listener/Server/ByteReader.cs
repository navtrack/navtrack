using System;
using System.Collections.Generic;
using System.Linq;

namespace Navtrack.Listener.Server
{
    public class ByteReader
    {
        private byte[] bytes;
        public int Index;

        public ByteReader(byte[] bytes)
        {
            this.bytes = bytes;
            Index = 0;
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
                value = BitConverter.ToInt64(Get(8));
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
                value = BitConverter.ToInt64(Get(8).Reverse().ToArray());
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
        
        public int GetMediumIntLe()
        {
            List<byte> a = Get(3).Reverse().ToList();
            a.Add(0);

            return BitConverter.ToInt32(a.ToArray());
        }
        
        private byte[] GetNext(int i, bool skipOne)
        {
            int endIndex = Index + i;
            
            byte[] sub = bytes[Index..endIndex];

            Index = skipOne ? Index + i + 1 : Index + i;

            return sub;
        } 

        public ByteReader Skip(int i)
        {
            if (i > 0)
            {
                Index += i;
            }

            return this;
        }

        public byte[] GetUntil(byte b, int? extra = null)
        {
            int newIndex = Array.IndexOf(bytes.Skip(Index).ToArray(), b);

            if (extra.HasValue)
            {
                newIndex += extra.Value;
            }
            
            return GetNext(newIndex, true);
        }

        public void Reset()
        {
            Index = 0;
        }

        public int BytesLeft => bytes.Length - Index;
    }
}