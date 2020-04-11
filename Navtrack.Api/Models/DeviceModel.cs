using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Models
{
    public class DeviceModel : IModel
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        [MinLength(15)]
        public string IMEI { get; set; }

        public string Type { get; set; }
        
        public int DeviceTypeId { get; set; }
    }
}