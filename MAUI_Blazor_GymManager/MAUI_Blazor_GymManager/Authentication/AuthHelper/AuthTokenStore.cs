using Clients;

namespace MAUI_Blazor_GymManager.Authentication.AuthHelper
{
    public class AuthTokenStore : IAuthTokenStore
    {
        private DateTime _tokenExpiration = DateTime.MinValue;
        private Task<string> _lastFetchTask;

        public async Task<string> GetOrFetchAccessTokenAsync(Func<Task<TokenResponse>> fetchTokenFunc)
        {
            if (DateTime.UtcNow >= _tokenExpiration)
                _lastFetchTask = FetchToken(fetchTokenFunc);

            return await _lastFetchTask;
        }

        private async Task<string> FetchToken(Func<Task<TokenResponse>> fetchTokenFunc)
        {
            var tokenResponse = await fetchTokenFunc();
            _tokenExpiration = tokenResponse.ExpiresAt.DateTime;

            return tokenResponse.AccessToken;
        }
    }
}
