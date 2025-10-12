using Navtrack.Api.Model.Assets;

namespace Navtrack.Api.Services.Assets.Mappers;

public static class CreateAssetMapper
{
    public static void Map(CreateAssetModel source)
    {
        source.Name = source.Name.Trim();
    }
}