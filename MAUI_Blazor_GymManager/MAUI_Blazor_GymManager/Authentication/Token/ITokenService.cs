using Clients;

namespace MAUI_Blazor_GymManager.Authentication.Token
{
    public interface ITokenService
    {
        Task<TokenResponse> RefreshAccessTokenAsync();
    }
}
