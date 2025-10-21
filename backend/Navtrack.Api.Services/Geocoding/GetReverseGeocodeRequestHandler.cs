using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Navtrack.Api.Model.Geocode;
using Navtrack.Api.Services.Geocoding.Mappers;
using Navtrack.Api.Services.Geocoding.Models;
using Navtrack.Api.Services.Requests;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Geocoding;

[Service(typeof(IRequestHandler<GetReverseGeocodeRequest, LocationModel>))]
public class GetReverseGeocodeRequestHandler(IConfiguration configuration)
    : BaseRequestHandler<GetReverseGeocodeRequest, LocationModel>
{
    public override async Task<LocationModel> Handle(GetReverseGeocodeRequest request)
    {
        Place? place = await GetPlace(request.Latitude, request.Longitude);

        LocationModel model = LocationModelMapper.Map(place);

        return model;
    }

    private async Task<Place?> GetPlace(double latitude, double longitude)
    {
        string? nominatimUrl = configuration["NominatimUrl"];
        
        if (!string.IsNullOrEmpty(nominatimUrl) && latitude != 0 && longitude != 0)
        {
            try
            {
                HttpClient httpClient = new();

                string url =
                    $"{nominatimUrl}/reverse?format=jsonv2&lon={Convert.ToString(longitude, CultureInfo.InvariantCulture)}&lat={Convert.ToString(latitude, CultureInfo.InvariantCulture)}";

                Place? place = await httpClient.GetFromJsonAsync<Place>(url);

                return place;
            }
            catch (Exception)
            {
                return null;
            }
        }

        return null;
    }
}