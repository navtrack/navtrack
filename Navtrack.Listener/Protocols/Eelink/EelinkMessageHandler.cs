using System;
using System.Linq;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Eelink
{
    [Service(typeof(ICustomMessageHandler<EelinkProtocol>))]
    public class EelinkMessageHandler : BaseMessageHandler<EelinkProtocol>
    {
        public override Location Parse(MessageInput input)
        {
            PackageIdentifier packageIdentifier = (PackageIdentifier) input.DataMessage.Bytes[2];

            if (packageIdentifier == PackageIdentifier.Login)
            {
                HandleLoginPackage(input);
            }
            else if (packageIdentifier == PackageIdentifier.Heartbeat)
            {
                HandleHeartbeatPackage(input);
            }
            else if (packageIdentifier == PackageIdentifier.Location)
            {
                return HandleLocationPackage(input);
            }
            else if (packageIdentifier == PackageIdentifier.Warning)
            {
                return HandleWarningPackage(input);
            }
            else if (packageIdentifier == PackageIdentifier.Report)
            {
                return HandleReportPackage(input);
            }
            else if (packageIdentifier == PackageIdentifier.Message)
            {
                return HandleMessagePackage(input);
            }
            else if (packageIdentifier == PackageIdentifier.ParamSet)
            {
                HandleParamSetPackage(input);
            }

            return null;
        }

        private void HandleParamSetPackage(MessageInput input)
        {
            const string mark = "6767";
            string pid = input.DataMessage.Hex[2];
            const int size = 3;
            string sequence = input.DataMessage.Hex[5..7].StringJoin();

            string next = "00";

            string reply = $"{mark}{pid}{size:X4}{sequence}{next}";

            input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(reply));
        }

        private Location HandleMessagePackage(MessageInput input)
        {
            Location location = GetLocation(input, 7);

            const string mark = "6767";
            string pid = input.DataMessage.Hex[2];
            const int size = 23;
            string sequence = input.DataMessage.Hex[5..7].StringJoin();

            string number =
                HexUtil.ConvertHexStringArrayToHexString(
                    HexUtil.ConvertByteArrayToHexStringArray(input.DataMessage.ByteReader.Get(21)));

            string reply = $"{mark}{pid}{size:X4}{sequence}{number}";

            input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(reply));

            return location;
        }

        private Location HandleReportPackage(MessageInput input)
        {
            Location location = GetLocation(input, 7);

            const string mark = "6767";
            string pid = input.DataMessage.Hex[2];
            const int size = 2;
            string sequence = input.DataMessage.Hex[5..7].StringJoin();

            string reply = $"{mark}{pid}{size:X4}{sequence}";

            input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(reply));

            return location;
        }

        private Location HandleWarningPackage(MessageInput input)
        {
            Location location = GetLocation(input, 4);

            const string mark = "6767";
            string pid = input.DataMessage.Hex[2];
            const int size = 2;
            string sequence = input.DataMessage.Hex[5..7].StringJoin();

            string reply = $"{mark}{pid}{size:X4}{sequence}";

            input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(reply));

            return location;
        }

        private static Location HandleLocationPackage(MessageInput input)
        {
            Location location = GetLocation(input, 7);

            const string mark = "6767";
            string pid = input.DataMessage.Hex[2];
            const int size = 2;
            string sequence = input.DataMessage.Hex[5..7].StringJoin();

            string reply = $"{mark}{pid}{size:X4}{sequence}";

            input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(reply));

            return location;
        }

        private static Location GetLocation(MessageInput input, int startIndex)
        {
            Location location = new Location
            {
                Device = input.Client.Device,
            };

            input.DataMessage.ByteReader.Skip(startIndex);
            location.DateTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(
                BitConverter.ToInt32(input.DataMessage.ByteReader.Get(4)));
            string mask = Convert.ToString(input.DataMessage.ByteReader.GetOne(), 2).PadLeft(8, '0');

            if (mask[^1] == '1')
            {
                location.Latitude = BitConverter.ToInt32(input.DataMessage.ByteReader.Get(4).Reverse().ToArray()) / 1800000.0m;
                location.Longitude = BitConverter.ToInt32(input.DataMessage.ByteReader.Get(4).Reverse().ToArray()) / 1800000.0m;
                location.Altitude = BitConverter.ToInt16(input.DataMessage.ByteReader.Get(2).Reverse().ToArray());
                location.Speed = BitConverter.ToInt16(input.DataMessage.ByteReader.Get(2).Reverse().ToArray());
                location.Heading = BitConverter.ToInt16(input.DataMessage.ByteReader.Get(2).Reverse().ToArray());
                location.Satellites = Convert.ToInt16(input.DataMessage.ByteReader.GetOne());
            }

            if (mask[^2] == '1')
            {
                location.MobileCountryCode = BitConverter.ToInt16(input.DataMessage.ByteReader.Get(2).Reverse().ToArray());
                location.MobileNetworkCode = BitConverter.ToInt16(input.DataMessage.ByteReader.Get(2).Reverse().ToArray());
                location.LocationAreaCode = BitConverter.ToInt16(input.DataMessage.ByteReader.Get(2).Reverse().ToArray());
                location.CellId = BitConverter.ToInt32(input.DataMessage.ByteReader.Get(4).Reverse().ToArray());
                location.GsmSignal = input.DataMessage.ByteReader.GetOne();
            }

            if (mask[^3] == '1')
            {
                input.DataMessage.ByteReader.Skip(7);
            }

            if (mask[^4] == '1')
            {
                input.DataMessage.ByteReader.Skip(7);
            }

            if (mask[^5] == '1')
            {
                input.DataMessage.ByteReader.Skip(7);
            }

            if (mask[^6] == '1')
            {
                input.DataMessage.ByteReader.Skip(7);
            }

            if (mask[^7] == '1')
            {
                input.DataMessage.ByteReader.Skip(7);
            }

            return location;
        }

        private static void HandleLoginPackage(MessageInput input)
        {
            const string mark = "6767";
            const int size = 9;
            string pid = input.DataMessage.Hex[2];
            string sequence = input.DataMessage.Hex[5..7].StringJoin();
            const int version = 1;
            const string paramSetAction = "03";
            int time = (int) (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;

            string reply = $"{mark}{pid}{size:X4}{sequence}{time:X4}{version:X4}{paramSetAction}";

            input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(reply));
        }

        private static void HandleHeartbeatPackage(MessageInput input)
        {
            const string mark = "6767";
            string pid = input.DataMessage.Hex[2];
            const int size = 2;
            string sequence = input.DataMessage.Hex[5..7].StringJoin();

            string reply = $"{mark}{pid}{size:X4}{sequence}";

            input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(reply));
        }
    }
}