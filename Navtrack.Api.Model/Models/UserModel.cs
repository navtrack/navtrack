using System.ComponentModel.DataAnnotations;
using Navtrack.Api.Model.Custom;

namespace Navtrack.Api.Model.Models
{
    public class UserModel : IModel
    {
        public int Id { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public string ConfirmPassword { get; set; }
        
        public UserRole? Role { get; set; }
    }
}