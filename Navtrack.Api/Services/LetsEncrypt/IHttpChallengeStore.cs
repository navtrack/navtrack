namespace Navtrack.Api.Services.LetsEncrypt
{
    internal interface IHttpChallengeStore
    {
        void AddChallengeResponse(string token, string response);
        bool TryGetResponse(string token, out string value);
    }
}