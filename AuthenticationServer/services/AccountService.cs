using AuthenticationServer.DTO.Login;
using AuthenticationServer.DTO.Logout;
using AuthenticationServer.DTO.Register;
using AuthenticationServer.Exceptions;
using AuthenticationServer.Models;
using AuthenticationServer.Repositories;
using AuthenticationServer.Services;

namespace AuthenticationServer.services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository userRepository;        
        private readonly IPasswordHasher passwordHasher;
        public AccountService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
        }
        public Task Login(LoginRequest loginRequest)
        {
            try
            {
                if (loginRequest == null)
                    throw new LoginException("User login parameter is null");

                if (!IsUserValid)
                    throw new LoginException("Invalid login/password");




            }
            catch (Exception ex) 
            {
                throw new LoginException(ex.Message);
            }
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
        private async Task<bool> IsUserValid(LoginRequest loginRequest)
        {
            User user = await userRepository.GetByEmailAsync(loginRequest.Email);
            if (user == null)
                return false;

            if (!passwordHasher.Verify(loginRequest.Password, user.PasswordHash))
                return false;

            return true;
        }
    }
}
