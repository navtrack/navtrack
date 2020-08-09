using Navtrack.Api.Model.Accounts;
using Navtrack.Api.Model.Models;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Mappers
{
    [Service(typeof(IMapper<UserEntity, AccountInfoResponseModel>))]
    public class AccountInfoResponseModelMapper : IMapper<UserEntity, AccountInfoResponseModel>
    {
        public AccountInfoResponseModel Map(UserEntity source, AccountInfoResponseModel destination)
        {
            destination.Id = source.Id;
            destination.Email = source.Email;
            destination.Role = (UserRole) source.Role;

            return destination;
        }
    }
}