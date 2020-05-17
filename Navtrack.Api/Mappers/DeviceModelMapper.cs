using Navtrack.Api.Models;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using DeviceModel = Navtrack.DeviceData.Model.DeviceModel;

namespace Navtrack.Api.Mappers
{
    [Service(typeof(IMapper<DeviceModel, DeviceModelModel>))]
    public class DeviceModelMapper : IMapper<DeviceModel, DeviceModelModel>
    {
        public DeviceModelModel Map(DeviceModel source, DeviceModelModel destination)
        {
            destination.Id = source.Id;
            destination.DisplayName = $"{source.Manufacturer} {source.Model}";
            destination.Manufacturer = source.Manufacturer;
            destination.Model = source.Model;
            destination.Protocol = source.Protocol;
            destination.Port = source.Port;

            return destination;
        }
    }
}