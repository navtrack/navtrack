using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.DeviceData.Model;
using Navtrack.Library.DI;

namespace Navtrack.DeviceData.Services
{
    [Service(typeof(IDeviceTypeDataService), ServiceLifetime.Singleton)]
    public class DeviceTypeDataService : IDeviceTypeDataService
    {
        private DeviceType[] deviceTypes;
        private const string DeviceTypesFileName = "DeviceTypes.csv";

        public DeviceType GetById(int deviceTypeId)
        {
            return GetDeviceTypes().FirstOrDefault(x => x.Id == deviceTypeId);
        }

        public bool Exists(int deviceTypeId)
        {
            return GetDeviceTypes().Any(x => x.Id == deviceTypeId);
        }

        public IEnumerable<DeviceType> GetDeviceTypes()
        {
            if (deviceTypes == null)
            {
                string csv = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!,
                        DeviceTypesFileName));

                using StringReader stringReader = new StringReader(csv);

                string line;
                List<DeviceType> list = new List<DeviceType>();
                Dictionary<int, Protocol> protocols = new Dictionary<int, Protocol>();
                
                while ((line = stringReader.ReadLine()) != null)
                {
                    string[] split = line.Split(";");

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
                        Id = Convert.ToInt32(split[0]),
                        Manufacturer = split[1],
                        Type = split[2],
                        Protocol = protocols[port]
                    });
                }

                deviceTypes = list.ToArray();
            }

            return deviceTypes;
        }
    }
}