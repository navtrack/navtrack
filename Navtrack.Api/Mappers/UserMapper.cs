using System;
using Navtrack.Api.Models;
using Navtrack.Common.Services;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Mappers
{
    [Service(typeof(IMapper<User, UserModel>))]
    [Service(typeof(IMapper<UserModel, User>))]
    public class UserMapper : IMapper<User, UserModel>, IMapper<UserModel, User>
    {
        private readonly IPasswordHasher passwordHasher;

        public UserMapper(IPasswordHasher passwordHasher)
        {
            this.passwordHasher = passwordHasher;
        }

        public UserModel Map(User source, UserModel destination)
        {
            destination.Id = source.Id;
            destination.Email = source.Email;
            destination.Role = (UserRole) source.Role;

            return destination;
        }

        public User Map(UserModel source, User destination)
        {
            destination.Id = source.Id;
            destination.Email = source.Email;

            if (!string.IsNullOrEmpty(source.Password))
            {
                (string key, string salt) = passwordHasher.Hash(source.Password);

                destination.Hash = key;
                destination.Salt = salt;
            }

            if (source.Role.HasValue && Enum.TryParse($"{source.Role}", out UserRole role))
            {
                destination.Role = (int) role;
            }

            return destination;
        }
    }
}