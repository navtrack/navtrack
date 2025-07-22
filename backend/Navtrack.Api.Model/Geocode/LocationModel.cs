using System.Linq;

namespace Navtrack.Api.Model.Geocode;

public class LocationModel
{
    public string? Country { get; set; }
    public string? State { get; set; }
    public string? County { get; set; }
    public string? City { get; set; }
    public string? Town { get; set; }
    public string? Village { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }

    public string DisplayName => string.Join(", ", new[]
    {
        Country,
        State ?? County,
        City ?? Town ?? Village,
        Street ?? Suburb
    }.Where(x => !string.IsNullOrEmpty(x)));

    public string? Suburb { get; set; }
}