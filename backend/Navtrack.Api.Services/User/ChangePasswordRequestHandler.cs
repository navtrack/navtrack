using System.Net;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Account.Mappers;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.Passwords;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.User;

[Service(typeof(IRequestHandler<ChangePasswordRequest>))]
public class ChangePasswordRequestHandler(
    IUserRepository userRepository,
    INavtrackContextAccessor navtrackContextAccessor,
    IPasswordHasher passwordHasher)
    : BaseRequestHandler<ChangePasswordRequest>
{
    public override async Task Handle(ChangePasswordRequest request)
    {
        UserEntity? currentUser = navtrackContextAccessor.NavtrackContext?.User;
        currentUser.ThrowApiExceptionIfNull(HttpStatusCode.Unauthorized);

        ValidationApiException apiException = new();

        bool hasPassword = !string.IsNullOrEmpty(currentUser.PasswordHash) &&
                           !string.IsNullOrEmpty(currentUser.PasswordSalt);

        if (hasPassword && !passwordHasher.CheckPassword(request.Model.CurrentPassword,
                currentUser.PasswordHash,
                currentUser.PasswordSalt))
        {
            apiException.AddValidationError(nameof(request.Model.CurrentPassword),
                ApiErrorCodes.User_000008_InvalidCurrentPassword);
        }

        BasePasswordModelValidator.ValidatePasswords(request.Model, apiException, request.Model.CurrentPassword);

        apiException.ThrowIfInvalid();

        (string hash, string salt) = passwordHasher.Hash(request.Model.Password);

        await userRepository.Update(currentUser.Id, new UpdateUser
        {
            Password = new PasswordElement
            {
                Hash = hash,
                Salt = salt
            }
        });
    }
}