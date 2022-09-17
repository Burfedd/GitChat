using GitChat.Shared;

namespace GitChat.Client.Services
{
    public interface IAuthService
    {
        Task<LoginResultDTO> Login(LoginDTO loginModel);
        Task Logout();
        Task<RegisterResultDTO> Register(RegisterDTO registerModel);
    }
}
