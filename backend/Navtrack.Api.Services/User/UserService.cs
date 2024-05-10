using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.Mappers.Users;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.User;

[Service(typeof(IUserService))]
public class UserService(ICurrentUserAccessor userAccessor, IUserRepository repository) : IUserService
{
    public async Task<UserModel> GetCurrentUser()
    {
        UserDocument entity = await userAccessor.Get();

        return UserMapper.Map(entity);
    }

    public async Task Update(UpdateUserModel model)
    {
        UserDocument currentUser = await userAccessor.Get();
        UpdateUser updateUser = new();

        if (!string.IsNullOrEmpty(model.Email))
        {
            model.Email = model.Email.ToLower();

            if (currentUser.Email != model.Email)
            {
                if (await repository.EmailIsUsed(model.Email))
                {
                    throw new ValidationApiException().AddValidationError(nameof(UpdateUserModel.Email),
                        ApiErrorCodes.EmailAlreadyUsed);
                }

                updateUser.Email = model.Email;
            }
        }

        if (model.UnitsType.HasValue && currentUser.UnitsType != model.UnitsType)
        {
            updateUser.UnitsType = model.UnitsType;
        }

        await repository.Update(currentUser.Id, updateUser);
    }
}