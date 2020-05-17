using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Navtrack.Api.Models
{
    public class DeviceModel : IModel
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string DeviceId { get; set; }
        
        public int DeviceModelId { get; set; }
        
        [JsonPropertyName("deviceModel")]
        public DeviceModelModel Model { get; set; }
    }
}