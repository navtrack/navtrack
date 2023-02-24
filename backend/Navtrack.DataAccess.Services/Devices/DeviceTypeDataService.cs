using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Model.Protocols;
using Navtrack.Library.DI;

namespace Navtrack.DataAccess.Services.Devices;

[Service(typeof(IDeviceTypeDataService), ServiceLifetime.Singleton)]
public class DeviceTypeDataService : IDeviceTypeDataService
{
    private DeviceType[]? deviceTypes;

    public DeviceType GetById(string deviceTypeId)
    {
        return GetDeviceTypes().FirstOrDefault(x => x.Id == deviceTypeId);
    }

    public bool Exists(string deviceTypeId)
    {
        return GetDeviceTypes().Any(x => x.Id == deviceTypeId);
    }

    public IEnumerable<DeviceType> GetDeviceTypes()
    {
        if (deviceTypes == null)
        {
            using StringReader stringReader = new(DeviceTypes.DeviceTypesCsv);

            List<DeviceType> list = new();
            Dictionary<int, Protocol> protocols = new();

            while (stringReader.ReadLine() is { } line)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    string[] split = line.Split(";");

                    if (split.Length == 5)
                    {
                        int port = Convert.ToInt32(split[4]);

                        if (!protocols.ContainsKey(port))
                        {
                            protocols[port] = new Protocol
                            {
                                Name = split[3],
                                Port = port
                            };
                        }

                        list.Add(new DeviceType
                        {
                            Id = split[0],
                            Manufacturer = split[1],
                            Type = split[2],
                            Protocol = protocols[port]
                        });
                    }
                }
            }

            deviceTypes = list.ToArray();
        }

        return deviceTypes;
    }
}