using Navtrack.Api.Model.Geocode;
using Navtrack.Api.Services.Geocoding.Models;

namespace Navtrack.Api.Services.Geocoding.Mappers;

internal class LocationModelMapper
{
    public static LocationModel Map(Place? place)
    {
        return new LocationModel
        {
            Country = place?.Address?.Country,
            State = place?.Address?.County,
            County = place?.Address?.County,
            City = place?.Address?.City,
            Town = place?.Address?.Town,
            Village = place?.Address?.Village,
            Suburb = place?.Address?.Suburb,
            Street = place?.Address?.Road,
            PostalCode = place?.Address?.Postcode
        };
    }
}