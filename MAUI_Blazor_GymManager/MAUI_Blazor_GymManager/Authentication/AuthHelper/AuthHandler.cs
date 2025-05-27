using MAUI_Blazor_GymManager.Authentication.Token;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace MAUI_Blazor_GymManager.Authentication.AuthHelper
{
    public class AuthHandler(ITokenService tokenService,
                             IAuthTokenStore authTokenStore) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(HeaderNames.Authorization))
            {
                var token = await authTokenStore.GetOrFetchAccessTokenAsync(() => tokenService.RefreshAccessTokenAsync());
                request.Headers.TryAddWithoutValidation(HeaderNames.Authorization, $"Bearer {token}");
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var token = await authTokenStore.GetOrFetchAccessTokenAsync(() => tokenService.RefreshAccessTokenAsync());

                request.Headers.Remove(HeaderNames.Authorization);
                request.Headers.TryAddWithoutValidation(HeaderNames.Authorization, $"Bearer {token}");

                response = await base.SendAsync(request, cancellationToken);
            }

            return response;
        }
    }
}
