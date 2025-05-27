using Clients;

namespace MAUI_Blazor_GymManager.Authentication.AuthHelper
{
    public interface IAuthTokenStore
    {
        Task<string> GetOrFetchAccessTokenAsync(Func<Task<TokenResponse>> fetchTokenFunc);

    }
}
