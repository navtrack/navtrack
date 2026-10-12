using System.IO;
using Navtrack.Listener.Server;
using Xunit;

namespace Navtrack.Listener.Tests.Server;

public class ByteReaderTests
{
    [Fact]
    public void TruncatedField_IsRejectedWithoutAdvancingReader()
    {
        ByteReader reader = new([1, 2], ["01", "02"]);
        Assert.Throws<InvalidDataException>(() => reader.Get<int>());
        Assert.Equal(0, reader.Index);
        Assert.Equal(new byte[] { 1, 2 }, reader.Get(2));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(3)]
    [InlineData(int.MaxValue)]
    public void InvalidLengths_AreRejected(int length)
    {
        ByteReader reader = new([1, 2], ["01", "02"]);
        Assert.Throws<InvalidDataException>(() => reader.Get(length));
        Assert.Equal(0, reader.Index);
    }

    [Fact]
    public void MissingDelimiterAndTruncatedSkip_AreRejected()
    {
        ByteReader reader = new([1, 2], ["01", "02"]);
        Assert.Throws<InvalidDataException>(() => reader.GetUntil(0));
        Assert.Throws<InvalidDataException>(() => reader.Skip(3));
        Assert.Throws<InvalidDataException>(() => reader.GetHexStringArray(3));
        Assert.Equal(0, reader.Index);
    }
}
