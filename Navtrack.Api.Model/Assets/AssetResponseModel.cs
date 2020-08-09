using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Model.Locations;

namespace Navtrack.Api.Model.Assets
{
    public class AssetResponseModel : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DeviceModel ActiveDevice => Devices?.FirstOrDefault(x => x.IsActive);
        public IEnumerable<DeviceModel> Devices { get; set; }
        public LocationResponseModel Location { get; set; }
    }
}