using AuthenticationServer.DTO;
using AuthenticationServer.DTO.Login;
using AuthenticationServer.DTO.Logout;
using AuthenticationServer.DTO.Register;

namespace AuthenticationServer.services
{
    public interface IAccountService
    {
        public Task RegisterAsync(RegisterRequest registerRequest);
        public Task<AuthenticationResult> LoginAsync(LoginRequest loginRequest);
        public Task Logout(LogoutRequest logoutRequest);
        public Task<AuthenticationResult> RefreshTokenAsync(string refreshToken);
    }
}
