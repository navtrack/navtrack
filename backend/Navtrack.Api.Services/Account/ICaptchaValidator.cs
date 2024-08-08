using System.Threading.Tasks;

namespace Navtrack.Api.Services.Account;

public interface ICaptchaValidator
{
    Task<bool> Validate(string? captcha);
}