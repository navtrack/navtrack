using System.ComponentModel.DataAnnotations;

namespace Navtrack.Web.Model.Models
{
    public class AssetModel
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        public int DeviceId { get; set; }
        public string DeviceType { get; set; }
    }
}