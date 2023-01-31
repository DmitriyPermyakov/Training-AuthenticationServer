using AuthenticationServer.DTO.Login;
using AuthenticationServer.DTO.Logout;
using AuthenticationServer.DTO.Register;
using AuthenticationServer.Exceptions;
using AuthenticationServer.Models;
using AuthenticationServer.Repositories;

namespace AuthenticationServer.services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository userRepository;
        public AccountService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public Task Login(LoginRequest loginRequest)
        {
            throw new NotImplementedException();
        }

        public Task Logout(LogoutRequest logoutRequest)
        {
            throw new NotImplementedException();
        }

        public async Task Register(RegisterRequest registerRequest)
        {
            try
            {
                if (registerRequest == null)
                    throw new RegisterException("Register request parameter is null");

                User userFromDb = await userRepository.GetByEmailAsync(registerRequest.Email);

                if (userFromDb != null)
                    throw new RegisterException("User with this email is already exists");

                await userRepository.CreateAsync(registerRequest);
            }
            catch (InvalidOperationException ex)
            {
                throw new RegisterException(ex.Message);
            }
        }
    }
}
