using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Navtrack.Api.Model.Account;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Account.Mappers;
using Navtrack.Api.Services.Common.Email;
using Navtrack.Api.Services.Common.Email.Emails;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.Settings;
using Navtrack.Api.Services.Common.Settings.Models;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Account;

[Service(typeof(IRequestHandler<ForgotPasswordRequest>))]
public class ForgotPasswordRequestHandler(
    IHttpContextAccessor httpContextAccessor,
    IPasswordResetRepository passwordResetRepository,
    IUserRepository userRepository,
    IEmailService emailService,
    ISettingService settingService) : BaseRequestHandler<ForgotPasswordRequest>
{
    public override async Task Handle(ForgotPasswordRequest request)
    {
        string? ipAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        ipAddress.ReturnIfNull(HttpStatusCode.BadRequest);

        int passwordResetsIn24H =
            await passwordResetRepository.GetCountOfPasswordResets(ipAddress, request.Model.Email,
                DateTime.UtcNow.AddDays(-1));
        
        if (passwordResetsIn24H >= ApiConstants.MaxPasswordResetIn24Hours)
        {
            throw new ValidationApiException(nameof(ForgotPasswordModel.Email),
                ApiErrorCodes.User_MaxPasswordResetsExceeded);
        }

        UserEntity? user = await userRepository.GetByEmail(request.Model.Email);
        user.ReturnValidationErrorIfNull(nameof(ForgotPasswordModel.Email),
            ApiErrorCodes.User_EmailNotFound);

        await passwordResetRepository.MarkAsInvalidByUserId(user.Id);

        UserPasswordResetEntity document =
            PasswordResetEntityMapper.Map(request.Model.Email, user.Id, ipAddress);
                                                                
        await passwordResetRepository.Add(document);

        string url = await GetResetPasswordUrl(document);

        await emailService.Send(document.Email, new ResetPasswordEmail(url, 3));
    }

    private async Task<string> GetResetPasswordUrl(UserPasswordResetEntity entity)
    {
        AppSettings? appSettings = await settingService.Get<AppSettings>();

        if (!string.IsNullOrEmpty(appSettings?.AppUrl))
        {
            return ApiConstants.ResetPasswordLink(appSettings.AppUrl, entity.Id.ToString());
        }

        throw new ArgumentException("App URL not set.");
    }
}
