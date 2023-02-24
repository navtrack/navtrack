using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Navtrack.Api.Model;
using Navtrack.Api.Model.User;
using Navtrack.Api.Tests.Helpers;
using Navtrack.Common.Passwords;
using Navtrack.Common.Settings.Settings;
using Navtrack.DataAccess.Model.Settings;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Mongo;

namespace Navtrack.Api.Tests;

public class UserTests : BaseTest
{
    [Fact]
    public async Task ResetPassword_EmailNotInDatabase_ReturnsBadRequest()
    {
        using IServiceScope scope = factory.Services.CreateScope();

        HttpResponseMessage response = await httpClient.PostAsJsonAsync(ApiPaths.UserPasswordForgot,
            new ForgotPasswordRequest
            {
                Email = "nosuchemail@navtrack"
            });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ResetPassword_EmailIsInDatabase_ReturnsOk()
    {
        const string email = "choco@navtrack.com";

        using IServiceScope scope = factory.Services.CreateScope();
        IRepository? repository = scope.ServiceProvider.GetService<IRepository>();

        await repository!.GetCollection<UserDocument>().InsertOneAsync(new UserDocument
        {
            Email = email
        });
        
        await ConfigureAppUrl(repository);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync(ApiPaths.UserPasswordForgot,
            new ForgotPasswordRequest
            {
                Email = email
            });

        PasswordResetDocument passwordResetDocument = await repository.GetEntities<PasswordResetDocument>()
            .FirstOrDefaultAsync(x => x.Email == email);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(passwordResetDocument);
    }

    [Fact]
    public async Task ResetPassword_EmailIsInDatabase_EmailIsSent()
    {
        const string email = "choco@navtrack.com";

        using IServiceScope scope = factory.Services.CreateScope();
        IRepository? repository = scope.ServiceProvider.GetService<IRepository>();

        await repository!.GetCollection<UserDocument>().InsertOneAsync(new UserDocument
        {
            Email = email
        });
        
        await ConfigureAppUrl(repository);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync(ApiPaths.UserPasswordForgot,
            new ForgotPasswordRequest
            {
                Email = email
            });

        PasswordResetDocument passwordResetDocument = await repository.GetEntities<PasswordResetDocument>()
            .FirstOrDefaultAsync(x => x.Email == email);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(passwordResetDocument);
    }

    [Fact]
    public async Task ResetPassword_Make11Requests_LastOneReturnsBadRequest()
    {
        const string email = "choco@navtrack.com";

        using IServiceScope scope = factory.Services.CreateScope();
        IRepository? repository = scope.ServiceProvider.GetService<IRepository>();

        await repository!.GetCollection<UserDocument>().InsertOneAsync(new UserDocument
        {
            Email = email
        });

        List<HttpResponseMessage> responseMessages = new();

        for (int i = 0; i < 11; i++)
        {
            responseMessages.Add(await httpClient.PostAsJsonAsync(ApiPaths.UserPasswordForgot,
                new ForgotPasswordRequest
                {
                    Email = email
                }));
        }

        Assert.Equal(HttpStatusCode.TooManyRequests, responseMessages.Last().StatusCode);
    }
    
    
    [Fact]
    public async Task ResetPassword_LinkIsExpired_ReturnsBadRequest()
    {
        const string email = "choco@navtrack.com";

        using IServiceScope scope = factory.Services.CreateScope();
        IRepository? repository = scope.ServiceProvider.GetService<IRepository>();

        await ConfigureAppUrl(repository);

        await repository!.GetCollection<UserDocument>().InsertOneAsync(new UserDocument
        {
            Email = email
        });

        await httpClient.PostAsJsonAsync(ApiPaths.UserPasswordForgot,
            new ForgotPasswordRequest
            {
                Email = email
            });
        
        await repository.GetCollection<PasswordResetDocument>().UpdateOneAsync(x => x.Email == email,
            Builders<PasswordResetDocument>.Update.Set(x => x.Created.Date, DateTime.UtcNow.AddHours(-12)));

        PasswordResetDocument passwordResetDocument = await repository.GetEntities<PasswordResetDocument>()
            .FirstOrDefaultAsync(x => x.Email == email);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync(ApiPaths.UserPasswordReset,
            new ResetPasswordRequest
            {
                Hash = passwordResetDocument.Hash,
                Password = "new password",
                ConfirmPassword = "new password"
            });

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ResetPassword_CompleteFlow_PasswordIsChanged()
    {
        const string email = "choco@navtrack.com";

        using IServiceScope scope = factory.Services.CreateScope();
        IRepository? repository = scope.ServiceProvider.GetService<IRepository>();
        IPasswordHasher? passwordHasher = scope.ServiceProvider.GetService<IPasswordHasher>();

        (string? hash, string? salt) = passwordHasher!.Hash("old password");

        await ConfigureAppUrl(repository);

        await repository!.GetCollection<UserDocument>().InsertOneAsync(new UserDocument
        {
            Email = email,
            Password = new PasswordElement
            {
                Hash = hash,
                Salt = salt
            }
        });

        await httpClient.PostAsJsonAsync(ApiPaths.UserPasswordForgot,
            new ForgotPasswordRequest
            {
                Email = email
            });

        PasswordResetDocument passwordResetDocument = await repository.GetEntities<PasswordResetDocument>()
            .FirstOrDefaultAsync(x => x.Email == email);

        await httpClient.PostAsJsonAsync(ApiPaths.UserPasswordReset,
            new ResetPasswordRequest
            {
                Hash = passwordResetDocument.Hash,
                Password = "new password",
                ConfirmPassword = "new password"
            });

        UserDocument? user = await repository.GetEntities<UserDocument>()
            .FirstOrDefaultAsync(x => x.Id == passwordResetDocument.UserId);
        passwordResetDocument = await repository.GetEntities<PasswordResetDocument>()
            .FirstOrDefaultAsync(x => x.Email == email);

        Assert.True(passwordHasher.CheckPassword("new password", user.Password.Hash, user.Password.Salt));
        Assert.True(passwordResetDocument.Invalid);
    }
    
    [Fact]
    public async Task ResetPassword_Make2RequestAndTryToUseFirstHash_ReturnsBadRequest()
    {
        const string email = "choco@navtrack.com";

        using IServiceScope scope = factory.Services.CreateScope();
        IRepository? repository = scope.ServiceProvider.GetService<IRepository>();
        IPasswordHasher? passwordHasher = scope.ServiceProvider.GetService<IPasswordHasher>();

        await ConfigureAppUrl(repository);

        await repository!.GetCollection<UserDocument>().InsertOneAsync(new UserDocument
        {
            Email = email
        });

       await httpClient.PostAsJsonAsync(ApiPaths.UserPasswordForgot,
            new ForgotPasswordRequest
            {
                Email = email
            });
       await httpClient.PostAsJsonAsync(ApiPaths.UserPasswordForgot,
           new ForgotPasswordRequest
           {
               Email = email
           });

        PasswordResetDocument passwordResetDocument = await repository
            .GetEntities<PasswordResetDocument>()
            .Where(x => x.Email == email)
            .OrderBy(x => x.Created.Date)
            .FirstOrDefaultAsync();

        HttpResponseMessage response = await httpClient.PostAsJsonAsync(ApiPaths.UserPasswordReset,
            new ResetPasswordRequest
            {
                Hash = passwordResetDocument.Hash,
                Password = "new password",
                ConfirmPassword = "new password"
            });

        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ResetPassword_TryToUseHash2Times_SecondResponseIsBadRequest()
    {
        const string email = "choco@navtrack.com";

        using IServiceScope scope = factory.Services.CreateScope();
        IRepository? repository = scope.ServiceProvider.GetService<IRepository>();
        IPasswordHasher? passwordHasher = scope.ServiceProvider.GetService<IPasswordHasher>();

        (string? hash, string? salt) = passwordHasher!.Hash("old password");

        await ConfigureAppUrl(repository);

        await repository!.GetCollection<UserDocument>().InsertOneAsync(new UserDocument
        {
            Email = email,
            Password = new PasswordElement
            {
                Hash = hash,
                Salt = salt
            }
        });

        await httpClient.PostAsJsonAsync(ApiPaths.UserPasswordForgot,
            new ForgotPasswordRequest
            {
                Email = email
            });

        PasswordResetDocument passwordResetDocument = await repository.GetEntities<PasswordResetDocument>()
            .FirstOrDefaultAsync(x => x.Email == email);

        HttpResponseMessage firstResponse = await httpClient.PostAsJsonAsync(ApiPaths.UserPasswordReset,
            new ResetPasswordRequest
            {
                Hash = passwordResetDocument.Hash,
                Password = "new password",
                ConfirmPassword = "new password"
            });

        HttpResponseMessage secondResponse = await httpClient.PostAsJsonAsync(ApiPaths.UserPasswordReset,
            new ResetPasswordRequest
            {
                Hash = passwordResetDocument.Hash,
                Password = "new password",
                ConfirmPassword = "new password"
            });

        Assert.True(firstResponse.IsSuccessStatusCode);
        Assert.False(secondResponse.IsSuccessStatusCode);
    }
    
    private static async Task ConfigureAppUrl(IRepository? repository)
    {
        await repository!.GetCollection<SettingDocument>().InsertOneAsync(new SettingDocument
        {
            Key = "App",
            Value = new AppSettings { AppUrl = "http://test.navtrack.com" }.ToBsonDocument()
        });
    }
}