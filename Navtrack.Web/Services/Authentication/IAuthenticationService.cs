using System.Threading.Tasks;
using Navtrack.Web.Models;
using Navtrack.Web.Services.Generic;

namespace Navtrack.Web.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<ValidationResult> Login(LoginModel loginModel);
        Task Logout();
    }
}