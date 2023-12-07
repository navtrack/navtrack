using Navtrack.Api.Model.Assets;

namespace Navtrack.Api.Services.Mappers.Assets;

public static class CreateAssetModelMapper
{
    public static void Map(CreateAssetModel source)
    {
        source.Name = source.Name.Trim();
    }
}