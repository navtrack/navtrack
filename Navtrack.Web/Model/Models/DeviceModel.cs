using System.ComponentModel.DataAnnotations;

namespace Navtrack.Web.Model.Models
{
    public class DeviceModel
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