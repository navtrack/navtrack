using System;
using Navtrack.Api.Model.Models;
using Navtrack.Common.Services;
using Navtrack.DataAccess.Model;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services.Mappers
{
    [Service(typeof(IMapper<UserEntity, UserModel>))]
    [Service(typeof(IMapper<UserModel, UserEntity>))]
    public class UserMapper : IMapper<UserEntity, UserModel>, IMapper<UserModel, UserEntity>
    {
        private readonly IPasswordHasher passwordHasher;

        public UserMapper(IPasswordHasher passwordHasher)
        {
            this.passwordHasher = passwordHasher;
        }

        public UserModel Map(UserEntity source, UserModel destination)
        {
            destination.Id = source.Id;
            destination.Email = source.Email;
            destination.Role = (UserRole) source.Role;

            return destination;
        }

        public UserEntity Map(UserModel source, UserEntity destination)
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