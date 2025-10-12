using System;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Users;

namespace Navtrack.Api.Tests.Helpers;

public static class DatabaseSeed
{
    private static readonly Guid AuthenticatedUserId = Guid.NewGuid();
    private static readonly Guid AssetId = Guid.NewGuid();
    private static readonly Guid DeviceId = Guid.NewGuid();
    private static readonly Guid OrganizationId = Guid.NewGuid();

    public static readonly UserEntity AuthenticatedUser = new()
    {
        Id = AuthenticatedUserId,
        Email = "choco@navtrack.com",
        AssetUsers =
        [
            new AssetUserEntity
            {
                UserRole = AssetUserRole.Owner,
                AssetId = AssetId
            }
        ],
        OrganizationUsers = [
            new OrganizationUserEntity
            {
                OrganizationId = OrganizationId,
                UserRole = OrganizationUserRole.Owner
            }
        ]
    };

    public static readonly AssetEntity Asset = new()
    {
        Id = AssetId,
        Name = "Choco's car",
        OrganizationId = OrganizationId
    };

    public static readonly DeviceEntity Device = new DeviceEntity
    {
        Id = DeviceId,
        OrganizationId = OrganizationId,
        AssetId = AssetId,
        DeviceTypeId = "1",
        SerialNumber = "123456789",
        ProtocolPort = 7001
    };
    
    public static readonly OrganizationEntity Organization = new()
    {
        Id = OrganizationId,
        Name = "Choco & Milk Inc."
    };
}