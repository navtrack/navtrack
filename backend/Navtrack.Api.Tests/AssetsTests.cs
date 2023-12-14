using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Navtrack.Api.Model;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Tests.Helpers;

namespace Navtrack.Api.Tests;

public class AssetsTests(BaseTestFixture fixture) : BaseTest(fixture)
{
    [Fact]
    public async Task GetById_AssetAndUserInDatabase_ReturnsOk()
    {
        HttpResponseMessage response =
            await HttpClient.GetAsync(
                GetUrl(ApiPaths.AssetsAsset, new KeyValuePair<string, string>("assetId", Asset.Id.ToString())));

        AssetModel? asset = await response.GetResult<AssetModel>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(asset);
        Assert.Equal(Asset.Id.ToString(), asset.Id);
    }
}