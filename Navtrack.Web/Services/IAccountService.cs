using System.Threading.Tasks;
using Navtrack.Web.Models.Account;

namespace Navtrack.Web.Services
{
    public interface IAccountService
    {
        Task Login(LoginModel loginModel);
        Task Logout();
    }
}