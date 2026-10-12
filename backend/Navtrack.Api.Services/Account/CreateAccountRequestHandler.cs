using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Account.Mappers;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.Passwords;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Account;

[Service(typeof(IRequestHandler<CreateAccountRequest>))]
public class CreateAccountRequestHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ICaptchaValidator? captchaValidator = null) : BaseRequestHandler<CreateAccountRequest>
{
    public override async Task Handle(CreateAccountRequest request)
    {
        ValidationApiException validationException = new();
        
        validationException.AddErrorIfTrue(await userRepository.EmailIsUsed(request.Model.Email),
            nameof(request.Model.Email),
            ApiErrorCodes.User_EmailAlreadyUsed);

        validationException.AddErrorIfTrue(
            request.Model.Password != request.Model.ConfirmPassword,
            nameof(request.Model.ConfirmPassword),
            ApiErrorCodes.User_PasswordsNotEqual);

        validationException.AddErrorIfTrue(
            captchaValidator != null && !await captchaValidator.Validate(request.Model.Captcha),
            nameof(request.Model.Captcha),
            ApiErrorCodes.Validation_InvalidCaptcha);
        
        validationException.ThrowIfInvalid();
        
        (string hash, string salt) = passwordHasher.Hash(request.Model.Password);

        UserEntity user = UserDocumentMapper.Map(request.Model.Email, hash, salt);
        
        await userRepository.Add(user);
    }
}
