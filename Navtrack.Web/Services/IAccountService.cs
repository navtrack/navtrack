using System.Threading.Tasks;
using Navtrack.Web.Models;
using Navtrack.Web.Services.Generic;

namespace Navtrack.Web.Services
{
    public interface IAccountService
    {
        Task<ValidationResult> Register(RegisterModel registerModel);
    }
}