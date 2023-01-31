using AuthenticationServer.DTO.Login;
using AuthenticationServer.DTO.Logout;
using AuthenticationServer.DTO.Register;

namespace AuthenticationServer.services
{
    public interface IAccountService
    {
        public Task Register(RegisterRequest registerRequest);
        public Task Login(LoginRequest loginRequest);
        public Task Logout(LogoutRequest logoutRequest)
    }
}
