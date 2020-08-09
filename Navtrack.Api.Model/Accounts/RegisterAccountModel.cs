namespace Navtrack.Api.Model.Accounts
{
    public class RegisterAccountModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}