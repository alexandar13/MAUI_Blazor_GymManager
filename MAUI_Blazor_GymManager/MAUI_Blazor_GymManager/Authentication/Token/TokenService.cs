using Clients;

namespace MAUI_Blazor_GymManager.Authentication.Token
{
    public class TokenService(ITokenStorage tokenStorage,
                              IAuthApi client) : ITokenService
    {
        public async Task<TokenResponse> RefreshAccessTokenAsync()
        {
            var refreshToken = await tokenStorage.GetRefreshTokenAsync();
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new InvalidOperationException("No refresh token stored.");
            }

            var response = await client.RefreshToken(new TokenRequest
            {
                RefreshToken = refreshToken
            });

            if (!string.IsNullOrEmpty(response.Data.RefreshToken))
            {
                await tokenStorage.SaveRefreshTokenAsync(response.Data.RefreshToken);
            }

            return response.Data;
        }
    }
}
