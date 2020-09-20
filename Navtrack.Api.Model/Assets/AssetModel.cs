using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Custom;
using Navtrack.Api.Model.Devices;

namespace Navtrack.Api.Model.Assets
{
    public class AssetModel : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DeviceModel ActiveDevice => Devices?.FirstOrDefault(x => x.IsActive);
        public IEnumerable<DeviceModel> Devices { get; set; }
        public LocationModel Location { get; set; }
    }
}