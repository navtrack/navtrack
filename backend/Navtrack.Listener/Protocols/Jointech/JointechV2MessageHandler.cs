using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Jointech;

public static class JointechV2MessageHandler
{
    public static Position Parse(MessageInput input)
    {
        byte startFlag = input.DataMessage.ByteReader.GetByte();
        short messageId = input.DataMessage.ByteReader.GetInt16Be();
        short properties = input.DataMessage.ByteReader.GetInt16Be();
        string phoneNumber = input.DataMessage.ByteReader.GetHexString(6);
        short sequenceNumber = input.DataMessage.ByteReader.GetInt16Be();

        if (messageId == JointechV2Constants.LocationInformationReportMessageId)
        {
            input.ConnectionContext.SetDevice(phoneNumber);
            Position position = new()
            {
                Device = input.ConnectionContext.Device
            };

            int alarm = input.DataMessage.ByteReader.GetInt32Be();
            int status = input.DataMessage.ByteReader.GetInt32Be();
            position.Latitude = input.DataMessage.ByteReader.GetInt32Be() * 0.000001;
            position.Longitude = input.DataMessage.ByteReader.GetInt32Be() * 0.000001;
            position.Altitude = input.DataMessage.ByteReader.GetInt16Be();
            position.Speed = (float?)(input.DataMessage.ByteReader.GetInt16Be() * 0.1);
            position.Heading = input.DataMessage.ByteReader.GetInt16Be();

            if (BitUtil.IsTrue(status, 2))
            {
                position.Latitude = -position.Latitude;
            }

            if (BitUtil.IsTrue(status, 3))
            {
                position.Longitude = -position.Longitude;
            }

            int year = input.DataMessage.ByteReader.GetFromHex<int>(1);
            int month = input.DataMessage.ByteReader.GetFromHex<int>(1);
            int day = input.DataMessage.ByteReader.GetFromHex<int>(1);
            int hour = input.DataMessage.ByteReader.GetFromHex<int>(1);
            int minute = input.DataMessage.ByteReader.GetFromHex<int>(1);
            int second = input.DataMessage.ByteReader.GetFromHex<int>(1);

            position.Date = new DateTime(year + 2000, month, day, hour, minute, second)
                // default is GMT+8 time zone so we subtract that to have GMT
                .AddHours(-8);

            while (input.DataMessage.ByteReader.BytesLeft > 2)
            {
                byte additionalInformationId = input.DataMessage.ByteReader.GetByte();
                byte additionalInformationLength = input.DataMessage.ByteReader.GetByte();

                switch (additionalInformationId)
                {
                    case 0x30:
                        position.GsmSignal = input.DataMessage.ByteReader.GetByte();
                        break;
                    case 0x31:
                        position.Satellites = input.DataMessage.ByteReader.GetByte();
                        break;
                    default:
                        byte[] value = input.DataMessage.ByteReader.Get(additionalInformationLength);
                        break;
                }
            }

            byte checksum = input.DataMessage.ByteReader.GetByte();
            byte endFlag = input.DataMessage.ByteReader.GetByte();

            List<byte> response = SendResponse(phoneNumber, sequenceNumber, messageId, JointechV2Constants.ResponseSucceed);
            
            input.NetworkStream.Write(response.ToArray());

            return position;
        }

        return null;
    }

    private static List<byte> SendResponse(string phoneNumberHex, short sequenceNumber, short messageId, byte result)
    {
        List<byte> data = [];

        data.AddRange(BitConverter.GetBytes(sequenceNumber).Reverse());
        data.AddRange(BitConverter.GetBytes(messageId).Reverse());
        data.Add(result);

        MemoryStream stream = new();
        using BinaryWriter writer = new(stream);

        List<byte> bytes = [0x7e];

        bytes.AddRange(BitConverter.GetBytes(JointechV2Constants.GeneralResponseMessageId).Take(2).Reverse());
        
        bytes.AddRange(BitConverter.GetBytes(data.Count).Take(2).Reverse());

        bytes.AddRange(HexUtil.ConvertHexStringToByteArray(phoneNumberHex));

        bytes.AddRange(BitConverter.GetBytes((short)0).Reverse());
        
        bytes.AddRange(data);

        bytes.Add(ChecksumUtil.XorByte(bytes.Skip(1).ToArray()));

        bytes.Add(0x7e);

        return bytes;
    }
}