using Navtrack.Database.Model.Shared;

namespace Navtrack.Database.Model.Users;

public class UpdateUser
{
    public string? Email { get; set; }
    public UnitsType? UnitsType { get; set; }
    public PasswordElement? Password { get; set; }
    
    public string? AppleId { get; set; }
    public string? MicrosoftId { get; set; }
    public string? GoogleId { get; set; }
}