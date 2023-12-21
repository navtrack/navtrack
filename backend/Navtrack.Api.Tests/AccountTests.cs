using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Navtrack.Api.Model;
using Navtrack.Api.Model.User;
using Navtrack.Api.Tests.Helpers;
using static System.String;

namespace Navtrack.Api.Tests;

public class AccountTests(BaseTestFixture fixture) : BaseTest(fixture)
{
    [Fact]
    public async Task Register_SendRequestTwoTimes_SecondFailsWithEmailUsed()
    {
        RegisterAccountModel model = new()
        {
            Email = "email_used@navtrack.com",
            Password = "password",
            ConfirmPassword = "password"
        };

        HttpResponseMessage first =
            await HttpClient.PostAsJsonAsync(ApiPaths.Account, model);
        HttpResponseMessage second =
            await HttpClient.PostAsJsonAsync(ApiPaths.Account, model);

        Assert.Equal(HttpStatusCode.OK, first.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, second.StatusCode);
    }

    [Fact]
    public async Task Register_WithEmptyEmail_ReturnsValidationError()
    {
        RegisterAccountModel model = new()
        {
            Email = Empty,
            Password = "password",
            ConfirmPassword = "password",
        };

        HttpResponseMessage response =
            await HttpClient.PostAsJsonAsync(ApiPaths.Account, model);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}