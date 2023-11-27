using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Model.User;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.Extensions;
using Navtrack.Api.Services.Mappers.Users;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Services.Email;
using Navtrack.Shared.Services.Email.Emails;
using Navtrack.Shared.Services.Passwords;
using Navtrack.Shared.Services.Settings;
using Navtrack.Shared.Services.Settings.Settings;

namespace Navtrack.Api.Services.User;

[Service(typeof(IUserAccessService))]
public class UserAccessService : IUserAccessService
{
    private readonly IPasswordHasher passwordHasher;
    private readonly ICurrentUserAccessor currentUserAccessor;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IEmailService emailService;
    private readonly ISettingService settingService;
    private readonly IUserRepository userRepository;
    private readonly IPasswordResetRepository passwordResetRepository;

    public UserAccessService(IPasswordHasher passwordHasher, ICurrentUserAccessor currentUserAccessor,
        IHttpContextAccessor httpContextAccessor, IEmailService emailService, ISettingService settingService,
        IUserRepository userRepository, IPasswordResetRepository passwordResetRepository)
    {
        this.passwordHasher = passwordHasher;
        this.currentUserAccessor = currentUserAccessor;
        this.httpContextAccessor = httpContextAccessor;
        this.emailService = emailService;
        this.settingService = settingService;
        this.userRepository = userRepository;
        this.passwordResetRepository = passwordResetRepository;
    }

    public async Task ForgotPassword(ForgotPasswordModel model)
    {
        string? ipAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        ipAddress.ThrowApiExceptionIfNullOrEmpty(ApiErrorCodes.InvalidIpAddress);

        int passwordResetsIn24H =
            await passwordResetRepository.GetCountOfPasswordResets(ipAddress!, DateTime.UtcNow.AddDays(-1));

        if (passwordResetsIn24H >= ApiConstants.MaxPasswordResetIn24Hours)
        {
            throw new ApiException(ApiErrorCodes.MaxPasswordResetsExceeded, HttpStatusCode.TooManyRequests);
        }

        UserDocument? userDocument = await userRepository.GetByEmail(model.Email);

        if (userDocument == null)
        {
            throw new ApiException(ApiErrorCodes.Validation)
                .AddValidationError(nameof(ForgotPasswordModel.Email), ValidationErrorCodes.EmailDoesNotExist);
        }

        await passwordResetRepository.MarkAsInvalidByUserId(userDocument.Id);
        
        PasswordResetDocument document =
            PasswordResetMapper.Map(model.Email, userDocument.Id, ipAddress!);

        await passwordResetRepository.Add(document);

        string url = await GetResetPasswordUrl(document);

        await emailService.Send(document.Email, new ResetPasswordEmail(url, 3));
    }

    public async Task ChangePassword(ChangePasswordModel model)
    {
        UserDocument currentUser = await currentUserAccessor.Get();

        currentUser.ThrowApiExceptionIfNull(HttpStatusCode.Unauthorized);

        ApiException apiException = new();

        if (!passwordHasher.CheckPassword(model.CurrentPassword, currentUser.Password.Hash,
                currentUser.Password.Salt))
        {
            apiException.AddValidationError(nameof(model.CurrentPassword), ValidationErrorCodes.CurrentPasswordInvalid);
        }

        ValidatePasswords(model, apiException, model.CurrentPassword);

        apiException.ThrowIfInvalid();

        (string hash, string salt) = passwordHasher.Hash(model.Password);

        await userRepository.Update(currentUser.Id, new UpdateUser
        {
            Password = new PasswordElement
            {
                Hash = hash,
                Salt = salt
            }
        });
    }

    public async Task ResetPassword(ResetPasswordModel model)
    {
        ApiException apiException = new();

        ValidatePasswords(model, apiException);

        apiException.ThrowIfInvalid();

        PasswordResetDocument? passwordReset = await passwordResetRepository.GetLatestFromHash(model.Hash);

        if (passwordReset == null || passwordReset.Invalid || passwordReset.Hash != model.Hash)
        {
            throw new ApiException(ApiErrorCodes.PasswordResetInvalid);
        }

        if (passwordReset.Created.Date < DateTime.UtcNow.AddHours(-ApiConstants.PasswordResetLinkExpirationHours))
        {
            throw new ApiException(ApiErrorCodes.PasswordResetExpired);
        }

        (string hash, string salt) = passwordHasher.Hash(model.Password);

        await userRepository.Update(passwordReset.UserId, new UpdateUser
        {
            Password = new PasswordElement
            {
                Hash = hash,
                Salt = salt
            }
        });
        await passwordResetRepository.MarkAsInvalid(passwordReset.Id);
    }

    private static void ValidatePasswords(BasePasswordModel model, ApiException apiException,
        string? currentPassword = null)
    {
        if (model.Password != model.ConfirmPassword)
        {
            apiException.AddValidationError(nameof(model.ConfirmPassword), ValidationErrorCodes.PasswordsDoNotMatch);
        }
        else if (model.Password == currentPassword)
        {
            apiException.AddValidationError(nameof(model.Password), ValidationErrorCodes.PasswordMustBeNew);
        }
    }

    private async Task<string> GetResetPasswordUrl(PasswordResetDocument document)
    {
        AppSettings? appSettings = await settingService.Get<AppSettings>();

        if (!string.IsNullOrEmpty(appSettings?.AppUrl))
        {
            return ApiConstants.ResetPasswordLink(appSettings.AppUrl, document.Hash);
        }

        throw new ArgumentException("App URL not set.");
    }
}