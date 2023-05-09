using System;
using System.Collections.Generic;
using System.Linq;
using Navtrack.Listener.Helpers;

namespace Navtrack.Listener.Server;

public class ByteReader
{
    private byte[] bytes;
    private readonly string[] hex;
    public int Index;

    public ByteReader(byte[] bytes, string[] hex)
    {
        this.bytes = bytes;
        this.hex = hex;
        Index = 0;
    }

    public byte GetOne()
    {
        return GetNext(1, false).First();
    }


    public byte GetByte()
    {
        return Get(1)[0];
    }

    public byte[] Get(int i, bool count = true, bool reverse = false)
    {
        return GetNext(i, false, count, reverse);
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
        else if (typeof(T) == typeof(uint))
        {
            value = BitConverter.ToUInt32(Get(4));
        }

        return (T)value;
    }

    public int GetInt32Be()
    {
        return BitConverter.ToInt32(Get(4, reverse: true));
    }

    public short GetInt16Be()
    {
        return BitConverter.ToInt16(Get(2, reverse: true));
    }

    public T GetLe<T>(bool count = true)
    {
        object value = null;

        if (typeof(T) == typeof(short))
        {
            value = BitConverter.ToInt16(Get(2, count).Reverse().ToArray());
        }
        else if (typeof(T) == typeof(int))
        {
            value = BitConverter.ToInt32(Get(4, count).Reverse().ToArray());
        }
        else if (typeof(T) == typeof(long))
        {
            value = BitConverter.ToInt64(Get(8, count).Reverse().ToArray());
        }
        else if (typeof(T) == typeof(double))
        {
            value = BitConverter.ToDouble(Get(8, count).Reverse().ToArray());
        }
        else if (typeof(T) == typeof(decimal))
        {
            value = BitConverter.ToDouble(Get(16, count).Reverse().ToArray());
        }
        else if (typeof(T) == typeof(float))
        {
            value = BitConverter.ToSingle(Get(4, count).Reverse().ToArray());
        }

        return (T)value;
    }

    public int GetMediumIntLe()
    {
        List<byte> a = Get(3).Reverse().ToList();
        a.Add(0);

        return BitConverter.ToInt32(a.ToArray());
    }

    private byte[] GetNext(int i, bool skipOne, bool count = true, bool reverse = false)
    {
        int endIndex = Index + i;

        byte[] sub = bytes[Index..endIndex];

        if (count)
        {
            Index = skipOne ? Index + i + 1 : Index + i;
        }

        if (reverse)
        {
            Array.Reverse(sub);
        }

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

    public T? Get<T>(int i)
    {
        object? value = null;

        if (typeof(T) == typeof(string))
        {
            value = BitConverter.ToString(Get(i)).Replace("-", string.Empty);
        }

        return (T)value;
    }

    public string[] GetHexStringArray(int i)
    {
        int endIndex = Index + i;

        string[] sub = hex[Index..endIndex];

        Index = Index + i;

        return sub;
    }
    
    
    public string GetHexString(int i)
    {
        return string.Join("", GetHexStringArray(i));
    }


    public T? GetFromHex<T>(int i)
    {
        string x = BitConverter.ToString(Get(i)).Replace("-", string.Empty);

        return (T?)Convert.ChangeType(x, typeof(T));
    }
}