using Navtrack.Api.Models;
using Navtrack.Common.Services;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Mappers
{
    [Service(typeof(IMapper<RegisterModel, User>))]
    public class RegisterModelMapper : IMapper<RegisterModel, User>
    {
        private readonly IPasswordHasher passwordHasher;

        public RegisterModelMapper(IPasswordHasher passwordHasher)
        {
            this.passwordHasher = passwordHasher;
        }

        public User Map(RegisterModel source, User destination)
        {
            (string key, string hash) = passwordHasher.Hash(source.Password);

            destination.Email = source.Email;
            destination.Hash = key;
            destination.Salt = hash;

            return destination;
        }
    }
}