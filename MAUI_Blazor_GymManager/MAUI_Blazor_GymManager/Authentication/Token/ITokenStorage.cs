namespace MAUI_Blazor_GymManager.Authentication.Token
{
    public interface ITokenStorage
    {
        Task SaveRefreshTokenAsync(string token);
        Task<string> GetRefreshTokenAsync();
        void DeleteRefreshToken();
    }
}
