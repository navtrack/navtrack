using System.Threading.Tasks;
using Navtrack.Api.Models;
using Navtrack.Api.Services.Generic;

namespace Navtrack.Api.Services
{
    public interface IAccountService
    {
        Task<ValidationResult> Register(RegisterModel registerModel);
    }
}