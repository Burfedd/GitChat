using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace GitChat.Client.Handlers
{
    public class CustomAuthorizationHandler : DelegatingHandler
    {
        public ILocalStorageService _localStorageService { get; set; }
        public CustomAuthorizationHandler(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var jwtToken = await _localStorageService.GetItemAsync<string>("authToken");

            if (jwtToken != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}