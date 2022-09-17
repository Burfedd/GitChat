using Blazored.LocalStorage;
using GitChat.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace GitChat.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorageService;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorageService = localStorageService;
        }

        public async Task<RegisterResultDTO> Register(RegisterDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/accounts", dto);
            return await response.Content.ReadFromJsonAsync<RegisterResultDTO>();
        }

        public async Task<LoginResultDTO> Login(LoginDTO dto)
        {
            var loginAsJson = JsonSerializer.Serialize(dto);
            var response = await _httpClient.PostAsync("api/login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
            var loginResult = JsonSerializer.Deserialize<LoginResultDTO>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!response.IsSuccessStatusCode)
            {
                return loginResult;
            }

            await _localStorageService.SetItemAsync("authToken", loginResult.Token);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(dto.Username);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

            return loginResult;
        }

        public async Task Logout()
        {
            await _localStorageService.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
