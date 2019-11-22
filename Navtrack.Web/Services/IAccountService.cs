using System.Threading.Tasks;
using Navtrack.Web.Model.Account;

namespace Navtrack.Web.Services
{
    public interface IAccountService
    {
        Task Login(LoginModel loginModel);
        Task Logout();
    }
}