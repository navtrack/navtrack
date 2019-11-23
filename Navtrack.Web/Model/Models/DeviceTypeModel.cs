using System.ComponentModel.DataAnnotations;

namespace Navtrack.Web.Model.Models
{
    public class DeviceTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProtocolId { get; set; }
    }
}