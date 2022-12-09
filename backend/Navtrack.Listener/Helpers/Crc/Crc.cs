using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Navtrack.Listener.Helpers.Crc;

public class Crc : HashAlgorithm
{
    public static string ComputeHash(byte[] bytes, CrcAlgorithm crcAlgorithm)
    {
        Parameters parameters = CrcStdParams.Get(crcAlgorithm);
        Crc crc = new(parameters);

        IEnumerable<byte> result = crc.ComputeHash(bytes).Take(parameters.HashSize/8).Reverse();

        return HexUtil.ConvertHexStringArrayToHexString(HexUtil.ConvertByteArrayToHexStringArray(result));
    } 
        
    private readonly ulong mask;
    private readonly ulong[] table = new ulong[256];
    private ulong currentValue;

    private Crc(Parameters parameters)
    {
        Parameters = parameters ?? throw new ArgumentNullException("parameters");

        mask = ulong.MaxValue >> (64 - HashSize);

        Init();
    }

    public sealed override int HashSize => Parameters.HashSize;

    private Parameters Parameters { get; }

    private ulong[] GetTable()
    {
        ulong[] res = new ulong[table.Length];
        Array.Copy(table, res, table.Length);
        return res;
    }

    public override void Initialize()
    {
        currentValue = Parameters.RefOut ? ReverseBits(Parameters.Init, HashSize) : Parameters.Init;
    }

    protected override void HashCore(byte[] array, int ibStart, int cbSize)
    {
        currentValue = ComputeCrc(currentValue, array, ibStart, cbSize);
    }

    protected override byte[] HashFinal()
    {
        return BitConverter.GetBytes(currentValue ^ Parameters.XorOut);
    }

    private void Init()
    {
        CreateTable();
        Initialize();
    }

    private ulong ComputeCrc(ulong init, byte[] data, int offset, int length)
    {
        ulong crc = init;

        if (Parameters.RefOut)
        {
            for (int i = offset; i < offset + length; i++)
            {
                crc = table[(crc ^ data[i]) & 0xFF] ^ (crc >> 8);
                crc &= mask;
            }
        }
        else
        {
            int toRight = HashSize - 8;
            toRight = toRight < 0 ? 0 : toRight;
            for (int i = offset; i < offset + length; i++)
            {
                crc = table[((crc >> toRight) ^ data[i]) & 0xFF] ^ (crc << 8);
                crc &= mask;
            }
        }

        return crc;
    }

    private void CreateTable()
    {
        for (int i = 0; i < table.Length; i++)
            table[i] = CreateTableEntry(i);
    }

    private ulong CreateTableEntry(int index)
    {
        ulong r = (ulong)index;

        if (Parameters.RefIn)
            r = ReverseBits(r, HashSize);
        else if (HashSize > 8)
            r <<= HashSize - 8;

        ulong lastBit = 1ul << (HashSize - 1);

        for (int i = 0; i < 8; i++)
        {
            if ((r & lastBit) != 0)
                r = (r << 1) ^ Parameters.Poly;
            else
                r <<= 1;
        }

        if (Parameters.RefIn)
            r = ReverseBits(r, HashSize);

        return r & mask;
    }

    public static CheckResult[] CheckAll()
    {
        IEnumerable<Parameters> parameters = CrcStdParams.GetAll();

        List<CheckResult> result = new();
        foreach (Parameters parameter in parameters)
        {
            Crc crc = new(parameter);

            result.Add(new CheckResult
            {
                Parameter = parameter,
                Table = crc.GetTable()
            });
        }

        return result.ToArray();
    }

    public bool IsRight()
    {
        byte[] bytes = Encoding.ASCII.GetBytes("123456789");

        byte[] hashBytes = ComputeHash(bytes, 0, bytes.Length);

        ulong hash = BitConverter.ToUInt64(hashBytes, 0);

        return hash == Parameters.Check;
    }

    public class CheckResult
    {
        public Parameters Parameter { get; set; }

        public ulong[] Table { get; set; }
    }

    private static ulong ReverseBits(ulong ul, int valueLength)
    {
        ulong newValue = 0;

        for (int i = valueLength - 1; i >= 0; i--)
        {
            newValue |= (ul & 1) << i;
            ul >>= 1;
        }

        return newValue;
    }
}