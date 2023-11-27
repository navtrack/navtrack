using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.Mappers.Users;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Services.Passwords;

namespace Navtrack.Api.Services.User;

[Service(typeof(IUserService))]
public class UserService : IUserService
{
    private readonly IPasswordHasher passwordHasher;
    private readonly ICurrentUserAccessor currentUserAccessor;
    private readonly IUserRepository userRepository;

    public UserService(IPasswordHasher passwordHasher, ICurrentUserAccessor currentUserAccessor,
        IUserRepository userRepository)
    {
        this.passwordHasher = passwordHasher;
        this.currentUserAccessor = currentUserAccessor;
        this.userRepository = userRepository;
    }

    public async Task<Model.User.UserModel> GetCurrentUser()
    {
        UserDocument entity = await currentUserAccessor.Get();

        return UserMapper.Map(entity);
    }

    public async Task Update(UpdateUserModel model)
    {
        UserDocument currentUser = await currentUserAccessor.Get();
        UpdateUser updateUser = new();

        if (!string.IsNullOrEmpty(model.Email))
        {
            model.Email = model.Email.ToLower();

            if (currentUser.Email != model.Email)
            {
                if (await userRepository.EmailIsUsed(model.Email))
                {
                    throw new ValidationException().AddValidationError(nameof(UpdateUserModel.Email),
                        ValidationErrorCodes.EmailAlreadyUsed);
                }

                updateUser.Email = model.Email;
            }
        }
        
        if (model.UnitsType.HasValue && currentUser.UnitsType != model.UnitsType)
        {
            updateUser.UnitsType = model.UnitsType;
        }

        await userRepository.Update(currentUser.Id, updateUser);
    }

    public async Task Register(RegisterAccountModel model)
    {
        ApiException apiException = new();

        if (await userRepository.EmailIsUsed(model.Email))
        {
            apiException.AddValidationError(nameof(model.Email), ValidationErrorCodes.EmailAlreadyUsed);
        }

        if (model.Password != model.ConfirmPassword)
        {
            apiException.AddValidationError(nameof(model.ConfirmPassword),
                ValidationErrorCodes.PasswordsDoNotMatch);
        }

        apiException.ThrowIfInvalid();

        (string hash, string salt) = passwordHasher.Hash(model.Password);

        UserDocument userDocument = UserDocumentMapper.Map(model.Email, hash, salt);

        await userRepository.Add(userDocument);
    }
}