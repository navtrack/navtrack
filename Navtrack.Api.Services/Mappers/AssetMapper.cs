using Navtrack.Api.Model.Assets;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Common;

namespace Navtrack.Api.Services.Mappers;

public static class AssetMapper
{
    public static AssetModel Map(AssetDocument source, UnitsType unitsType)
    {
        AssetModel location = new()
        {
            Id = source.Id.ToString(),
            Name = source.Name,
            Location = LocationMapper.Map(source.Location, unitsType)
        };

        return location;
    }
}