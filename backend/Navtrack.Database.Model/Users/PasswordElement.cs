namespace Navtrack.Database.Model.Users;

public class PasswordElement
{
    public string? Hash { get; set; }

    public string? Salt { get; set; }
}