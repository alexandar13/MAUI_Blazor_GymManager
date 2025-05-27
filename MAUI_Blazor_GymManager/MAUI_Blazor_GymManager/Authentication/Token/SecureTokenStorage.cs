namespace MAUI_Blazor_GymManager.Authentication.Token
{
    public class SecureTokenStorage : ITokenStorage
    {
        private const string RefreshTokenKey = "refresh_token";

        public async Task SaveRefreshTokenAsync(string token)
        {
            await SecureStorage.SetAsync(RefreshTokenKey, token);
        }

        public async Task<string> GetRefreshTokenAsync()
        {
            try
            {
                return await SecureStorage.GetAsync(RefreshTokenKey);
            }
            catch (Exception ex)
            {
                // SecureStorage može baciti exception ako uređaj nema podršku ili je zaključan
                return null;
            }
        }

        public void DeleteRefreshToken()
        {
            SecureStorage.Remove(RefreshTokenKey);
        }
    }
}
