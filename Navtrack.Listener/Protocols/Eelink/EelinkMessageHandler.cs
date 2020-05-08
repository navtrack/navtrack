using System;
using System.Collections.Generic;
using System.Linq;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using static System.String;

namespace Navtrack.Listener.Protocols.Eelink
{
    [Service(typeof(ICustomMessageHandler<EelinkProtocol>))]
    public class EelinkMessageHandler : BaseMessageHandler<EelinkProtocol>
    {
        public override Location Parse(MessageInput input)
        {
            PackageIdentifier packageIdentifier = (PackageIdentifier) input.DataMessage.Bytes[2];

            Dictionary<PackageIdentifier, Func<Location>> dictionary = new Dictionary<PackageIdentifier, Func<Location>>
            {
                {
                    PackageIdentifier.Login, () =>
                    {
                        HandleLoginPackage(input);

                        return null;
                    }
                },
                {
                    PackageIdentifier.Heartbeat, () =>
                    {
                        SendAcknowledge(input);
                        return null;
                    }
                },
                {PackageIdentifier.Location, () => HandleLocationForV20(input)},
                {PackageIdentifier.Warning, () => HandleLocationForV20(input)},
                {PackageIdentifier.Report, () => HandleLocationForV20(input)},
                {
                    PackageIdentifier.Message, () =>
                    {
                        Location location = GetLocationV20(input);

                        string number =
                            HexUtil.ConvertHexStringArrayToHexString(
                                HexUtil.ConvertByteArrayToHexStringArray(input.DataMessage.ByteReader.Get(21)));

                        SendAcknowledge(input, number);

                        return location;
                    }
                },
                {
                    PackageIdentifier.ParamSet, () =>
                    {
                        SendAcknowledge(input, "00");

                        return null;
                    }
                },
                {PackageIdentifier.GPSOld, () => HandleMessageForV18(input, false)},
                {PackageIdentifier.AlarmOld, () => HandleMessageForV18(input)},
                {PackageIdentifier.TerminalStateOld, () => HandleMessageForV18(input)}
            };

            return dictionary.ContainsKey(packageIdentifier) ? dictionary[packageIdentifier].Invoke() : null;
        }

        private static Location HandleLocationForV20(MessageInput input)
        {
            Location location = GetLocationV20(input);

            SendAcknowledge(input);

            return location;
        }

        private static Location HandleMessageForV18(MessageInput input, bool sendAcknowledge = true)
        {
            Location location = GetLocationV18(input, 7);

            if (sendAcknowledge)
            {
                SendAcknowledge(input);
            }

            return location;
        }

        private static Location GetLocationV18(MessageInput input, int startIndex)
        {
            Location location = new Location
            {
                Device = input.Client.Device,
            };

            input.DataMessage.ByteReader.Skip(startIndex);
            location.DateTime = DateTime.UnixEpoch.AddSeconds(
                BitConverter.ToInt32(input.DataMessage.ByteReader.Get(4).Reverse().ToArray()));
            location.Latitude = BitConverter.ToInt32(input.DataMessage.ByteReader.Get(4).Reverse().ToArray()) /
                                1800000.0m;
            location.Longitude = BitConverter.ToInt32(input.DataMessage.ByteReader.Get(4).Reverse().ToArray()) /
                                 1800000.0m;
            location.Speed = BitConverter.ToInt16(input.DataMessage.ByteReader.Get(2).Reverse().ToArray());
            location.Heading = BitConverter.ToInt16(input.DataMessage.ByteReader.Get(2).Reverse().ToArray());
            byte[] mcc = input.DataMessage.ByteReader.Get(2);
            location.MobileCountryCode = BitConverter.ToInt16(mcc);
            location.MobileNetworkCode = BitConverter.ToInt16(input.DataMessage.ByteReader.Get(2));
            location.LocationAreaCode = BitConverter.ToInt16(input.DataMessage.ByteReader.Get(2));
            List<byte> cellId = input.DataMessage.ByteReader.Get(3).ToList();
            cellId.Insert(0, 0x00);
            location.CellId = BitConverter.ToInt32(cellId.ToArray());
            string status = Convert.ToString(input.DataMessage.ByteReader.GetOne(), 2).PadLeft(8, '0');
            location.PositionStatus = status[^1] == '1';

            return location;
        }

        private static Location GetLocationV20(MessageInput input)
        {
            const int locationStartIndex = 7;
            
            Location location = new Location
            {
                Device = input.Client.Device,
            };

            input.DataMessage.ByteReader.Skip(locationStartIndex);
            location.DateTime = DateTime.UnixEpoch.AddSeconds(
                BitConverter.ToInt32(input.DataMessage.ByteReader.Get(4).Reverse().ToArray()));
            string mask = Convert.ToString(input.DataMessage.ByteReader.GetOne(), 2).PadLeft(8, '0');

            if (mask[^1] == '1')
            {
                location.Latitude = BitConverter.ToInt32(input.DataMessage.ByteReader.Get(4).Reverse().ToArray()) /
                                    1800000.0m;
                location.Longitude = BitConverter.ToInt32(input.DataMessage.ByteReader.Get(4).Reverse().ToArray()) /
                                     1800000.0m;
                location.Altitude = BitConverter.ToInt16(input.DataMessage.ByteReader.Get(2).Reverse().ToArray());
                location.Speed = BitConverter.ToInt16(input.DataMessage.ByteReader.Get(2).Reverse().ToArray());
                location.Heading = BitConverter.ToInt16(input.DataMessage.ByteReader.Get(2).Reverse().ToArray());
                location.Satellites = Convert.ToInt16(input.DataMessage.ByteReader.GetOne());
            }

            if (mask[^2] == '1')
            {
                location.MobileCountryCode =
                    BitConverter.ToInt16(input.DataMessage.ByteReader.Get(2).Reverse().ToArray());
                location.MobileNetworkCode =
                    BitConverter.ToInt16(input.DataMessage.ByteReader.Get(2).Reverse().ToArray());
                location.LocationAreaCode =
                    BitConverter.ToInt16(input.DataMessage.ByteReader.Get(2).Reverse().ToArray());
                location.CellId = BitConverter.ToInt32(input.DataMessage.ByteReader.Get(4).Reverse().ToArray());
                location.GsmSignal = input.DataMessage.ByteReader.GetOne();
            }

            return location;
        }

        private static void HandleLoginPackage(MessageInput input)
        {
            string extra = Empty;

            // new protocol extra information
            if (input.DataMessage.Bytes.Length > 17)
            {
                const int version = 1;
                const string paramSetAction = "03";
                int time = (int) (DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds;

                extra = $"{time:X4}{version:X4}{paramSetAction}";
            }

            SendAcknowledge(input, $"{extra}");
        }

        private static void SendAcknowledge(MessageInput input, string extra = "")
        {
            const string mark = "6767";
            string pid = input.DataMessage.Hex[2];
            string sequence = input.DataMessage.Hex[5..7].StringJoin();
            int size = $"{sequence}{extra}".Length / 2;

            string reply = $"{mark}{pid}{size:X4}{sequence}{extra}";

            input.NetworkStream.Write(HexUtil.ConvertHexStringToByteArray(reply));
        }
    }
}