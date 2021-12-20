using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Assets;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Common;

namespace Navtrack.Api.Services.Mappers;

public static class AssetListMapper
{
    public static AssetListModel Map(IEnumerable<AssetDocument> source, UnitsType unitsType)
    {
        AssetListModel list = new()
        {
            Items = source
                .Select(x => new AssetModel
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Location = x.Location != null ? LocationMapper.Map(x.Location, unitsType) : null
                })
                .ToList()
        };
        return list;
    }
}