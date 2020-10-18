namespace Navtrack.Api.Model.Accounts
{
    public class RegisterAccountRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}