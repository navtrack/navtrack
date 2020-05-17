using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Navtrack.Api.Models;
using Navtrack.Api.Services.Generic;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;

namespace Navtrack.Api.Services
{
    [Service(typeof(IUserService))]
    [Service(typeof(IGenericService<DeviceEntity, DeviceModel>))]
    public class UserService : GenericService<UserEntity, UserModel>, IUserService
    {
        public UserService(IRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(
            repository, mapper, httpContextAccessor)
        {
        }

        public async Task<UserEntity> GetUserByEmail(string email)
        {
            UserEntity user = await repository.GetEntities<UserEntity>().FirstOrDefaultAsync(x => x.Email == email);

            return user;
        }

        public Task<bool> EmailIsUsed(string email)
        {
            return repository.GetEntities<UserEntity>().AnyAsync(x => x.Email == email);
        }

        protected override async Task ValidateSave(UserModel model, ValidationResult validationResult)
        {
            if (await repository.GetEntities<UserEntity>()
                    .AnyAsync(x => x.Id != model.Id && x.Email == model.Email))
            {
                validationResult.AddError(nameof(UserModel.Email), "Email is already used.");
            }

            if (model.Id <= 0 && string.IsNullOrEmpty(model.Password))
            {
                validationResult.AddError(nameof(UserModel.Password), "Password is required when adding a new user.");
            }
            if (model.Id <= 0 && string.IsNullOrEmpty(model.ConfirmPassword))
            {
                validationResult.AddError(nameof(UserModel.ConfirmPassword), "Confirm password is required when adding a new user.");
            }
            if (!string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.ConfirmPassword) &&
                model.Password != model.ConfirmPassword)
            {
                validationResult.AddError(nameof(UserModel.ConfirmPassword), "Passwords do no match.");
            }
        }
    }
}