using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.Api.Services.IdentityServer.Model;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.IdentityServer;

public class HandleTokenInput
{
    public HandleTokenInput(ExternalAuthenticationSettings settings)
    {
        MetadataAddress = settings.MetadataAddress;
        ValidAudience = settings.ClientId;
        ValidIssuer = settings.Issuer;
    }

    public string Token { get; set; }
    public string MetadataAddress { get; }
    public string ValidAudience { get; }
    public string ValidIssuer { get; }
    public string IdClaimType { get; set; }
    public string EmailClaimType { get; set; }
    public Func<ObjectId, string, Task> SetId { get; set; }
    public Func<UserDocument, string?> ExternalId { get; set; }
    public Func<string, string, UserDocument> Map { get; set; }
    public Func<string, string, Task<UserDocument?>> GetUser { get; set; }
}