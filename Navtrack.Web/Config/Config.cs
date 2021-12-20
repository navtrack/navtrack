namespace Navtrack.Web.Config;

public class Config
{
    public string? ApiUrl { get; set; }
    public string? MapTileUrl { get; set; }
    public string? SentryDsn { get; set; }
    public string Environment { get; set; }
    public string? Release { get; set; }
}