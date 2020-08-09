using Navtrack.Api.Model.Accounts;
using Navtrack.Common.Services;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Accounts
{
    [Service(typeof(IMapper<RegisterAccountModel, UserEntity>))]
    public class RegisterAccountModelToUserEntityMapper : IMapper<RegisterAccountModel, UserEntity>
    {
        private readonly IPasswordHasher passwordHasher;

        public RegisterAccountModelToUserEntityMapper(IPasswordHasher passwordHasher)
        {
            this.passwordHasher = passwordHasher;
        }

        public UserEntity Map(RegisterAccountModel source, UserEntity destination)
        {
            (string key, string hash) = passwordHasher.Hash(source.Password);

            destination.Email = source.Email;
            destination.Hash = key;
            destination.Salt = hash;

            return destination;
        }
    }
}