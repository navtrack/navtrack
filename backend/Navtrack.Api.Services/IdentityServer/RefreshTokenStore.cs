using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Navtrack.Api.Services.Mappers;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Library.DI;

namespace Navtrack.Api.Services.IdentityServer;

[Service(typeof(IRefreshTokenStore))]
public class RefreshTokenStore : IRefreshTokenStore
{
    private readonly ITokenDataService tokenDataService;

    public RefreshTokenStore(ITokenDataService tokenDataService)
    {
        this.tokenDataService = tokenDataService;
    }

    public async Task<string> StoreRefreshTokenAsync(RefreshToken refreshToken)
    {
        RefreshTokenDocument document = RefreshTokenDocumentMapper.Map(refreshToken);

        await tokenDataService.Add(document);

        return document.Id.ToString();
    }

    public Task UpdateRefreshTokenAsync(string handle, RefreshToken refreshToken)
    {
        RefreshTokenDocument document = RefreshTokenDocumentMapper.Map(refreshToken);

        return tokenDataService.Add(document);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshTokenHandle)
    {
        RefreshTokenDocument? document = await tokenDataService.GetByUserId(refreshTokenHandle);

        if (document != null)
        {
            RefreshToken refreshToken = RefreshTokenMapper.Map(document);

            return refreshToken;
        }
        
        return null;
    }

    public Task RemoveRefreshTokenAsync(string refreshTokenHandle)
    {
        return tokenDataService.Remove(refreshTokenHandle);
    }

    public Task RemoveRefreshTokensAsync(string subjectId, string clientId)
    {
        return tokenDataService.Remove(subjectId);
    }
}