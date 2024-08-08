namespace Navtrack.Api.Model.Account;

public class ChangePasswordModel : BasePasswordModel
{
    public string CurrentPassword { get; set; }
}