using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.DeviceData.Model;
using Navtrack.Library.DI;

namespace Navtrack.DeviceData.Services
{
    [Service(typeof(IDeviceModelDataService), ServiceLifetime.Singleton)]
    public class DeviceModelDataService : IDeviceModelDataService
    {
        private DeviceModel[] deviceTypes;
        private const string DeviceModelsFileName = "DeviceModels.json";

        public DeviceModel GetById(int id)
        {
            return GetDeviceModels().FirstOrDefault(x => x.Id == id);
        }

        public DeviceModel[] GetDeviceModels()
        {
            if (deviceTypes == null)
            {
                string json = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!,
                        DeviceModelsFileName));

                deviceTypes = JsonSerializer.Deserialize<DeviceModel[]>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }).OrderBy(x => x.Manufacturer)
                    .ThenBy(x => x.Model)
                    .ToArray();
            }

            return deviceTypes;
        }
    }
}