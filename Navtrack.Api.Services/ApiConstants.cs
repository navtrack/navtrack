namespace Navtrack.Api.Services
{
    public class ApiConstants
    {
        public static string HubUrl(string hub = "") => $"/api/hubs/{hub}";
    }
}