using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Navtrack.Api.Services.Common.IdentityServer.Mappers;
using Navtrack.Database.Model.Authentication;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Common.IdentityServer;

[Service(typeof(IRefreshTokenStore))]
public class RefreshTokenStore(IRefreshTokenRepository tokenRepository) : IRefreshTokenStore
{
    public async Task<string> StoreRefreshTokenAsync(RefreshToken refreshToken)
    {
        AuthRefreshTokenEntity entity = RefreshTokenEntityMapper.Map(refreshToken);
        
        await tokenRepository.Add(entity);

        return entity.Hash;
    }

    public Task UpdateRefreshTokenAsync(string? handle, RefreshToken refreshToken)
    {
        return Task.CompletedTask;
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshTokenHandle)
    {
        AuthRefreshTokenEntity? document = await tokenRepository.Get(refreshTokenHandle);

        if (document != null)
        {
            RefreshToken refreshToken = RefreshTokenMapper.Map(document);

            return refreshToken;
        }
        
        return null;
    }

    public Task RemoveRefreshTokenAsync(string refreshTokenHandle)
    {
        return tokenRepository.Remove(refreshTokenHandle);
    }

    public Task RemoveRefreshTokensAsync(string subjectId, string clientId)
    {
        return tokenRepository.Remove(subjectId, clientId);
    }
}