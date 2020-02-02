using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Navtrack.DataAccess.Model;
using Navtrack.DataAccess.Repository;
using Navtrack.Library.DI;
using Navtrack.Library.Services;
using Navtrack.Web.Models;
using Navtrack.Web.Services.Generic;

namespace Navtrack.Web.Services
{
    [Service(typeof(IUserService))]
    [Service(typeof(IGenericService<Device, DeviceModel>))]
    public class UserService : GenericService<User, UserModel>, IUserService
    {
        public UserService(IRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(
            repository, mapper, httpContextAccessor)
        {
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User user = await repository.GetEntities<User>().FirstOrDefaultAsync(x => x.Email == email);

            return user;
        }

        public Task<bool> EmailIsUsed(string email)
        {
            return repository.GetEntities<User>().AnyAsync(x => x.Email == email);
        }

        protected override async Task ValidateSave(UserModel model, ValidationResult validationResult)
        {
            if (await repository.GetEntities<User>()
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
                model.Password == model.ConfirmPassword)
            {
                validationResult.AddError(nameof(UserModel.ConfirmPassword), "Passwords do no match.");
            }
        }
    }
}