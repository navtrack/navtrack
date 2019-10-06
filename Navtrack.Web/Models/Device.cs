using System.ComponentModel.DataAnnotations;

namespace Navtrack.Web.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        [Required]
        [MinLength(15)]
        [MaxLength(16)]
        public string IMEI { get; set; }
    }
}