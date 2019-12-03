using System.ComponentModel.DataAnnotations;

namespace Navtrack.Web.Models
{
    public class UserModel : IModel
    {
        public int Id { get; set; }
        
        [Required]
        public string Email { get; set; }
    }
}