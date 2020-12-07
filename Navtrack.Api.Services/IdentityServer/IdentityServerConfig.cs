using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Navtrack.Api.Services.IdentityServer
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }
        

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new()
                {
                    ClientId = "navtrack.web",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                        IdentityServerConstants.LocalApi.ScopeName,
                        IdentityServerConstants.StandardScopes.OpenId
                    },
                    RequireClientSecret = false,
                    AllowOfflineAccess = true
                }
            };
        }

        public static IEnumerable<ApiScope> GetScopes()
        {
            return new List<ApiScope> { new(IdentityServerConstants.LocalApi.ScopeName) };
        }
    }
}