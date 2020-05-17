using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Models
{
    public class AssetModel : IModel
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        public int DeviceId { get; set; }
        
        public LocationModel Location { get; set; }
        public DeviceModel Device { get; set; }
    }
}