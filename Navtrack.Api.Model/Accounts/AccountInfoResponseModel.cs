using Navtrack.Api.Model.Models;

namespace Navtrack.Api.Model.Accounts
{
    public class AccountInfoResponseModel
    {
        public int Id { get; set; }
        
        public string Email { get; set; }
        
        public UserRole Role { get; set; }
    }
}