using AuthenticationServer.DTO;
using AuthenticationServer.DTO.Login;
using AuthenticationServer.DTO.Logout;
using AuthenticationServer.DTO.Register;
using AuthenticationServer.Exceptions;
using AuthenticationServer.JwtSettingsParameters;
using AuthenticationServer.ValidationParametersFactory;
using AuthenticationServer.Models;
using AuthenticationServer.Repositories;
using AuthenticationServer.Services;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationServer.services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository userRepository;        
        private readonly IPasswordHasher passwordHasher;
        private readonly ITokenGenerator tokenGenerator;
        private readonly ITokenRepository tokenRepository;
        private readonly JwtSettings jwtSettings;
        public AccountService(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator, JwtSettings jwtSettings, ITokenRepository tokenRepository)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.tokenGenerator = tokenGenerator;
            this.jwtSettings = jwtSettings;
            this.tokenRepository = tokenRepository;
        }
        public async Task<AuthenticationResult> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                if (loginRequest == null)
                    throw new LoginException("User login parameter is null");

                User user = await userRepository.GetByEmailAsync(loginRequest.Email);

                if (user == null)
                    throw new LoginException("Invalid login/password");

                if (!passwordHasher.Verify(loginRequest.Password, user.PasswordHash))
                    throw new LoginException("Invalid login/password");

                string accessToken = await tokenGenerator.GenerateTokenAsync(TokenType.AccessToken, user);
                string refreshToken = await tokenGenerator.GenerateTokenAsync(TokenType.RefreshToken, user);

                return new AuthenticationResult()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };

            }
            catch (Exception ex) 
            {
                throw new LoginException(ex.Message);
            }
        }

        public async Task Logout(LogoutRequest logoutRequest)
        {
            var tokenValidationParameters = new ValidationParametersFactory.ValidationParametersFactory(jwtSettings).AccessTokenValidationParameters;

            SecurityToken validatedToken = tokenGenerator.ValidateToken(logoutRequest.AccessToken, tokenValidationParameters);

            if (validatedToken == null)
                throw new LogoutException("Invalid refresh token");

            var refreshToken = await tokenRepository.GetByTokenAsync(logoutRequest.AccessToken);
            if (refreshToken == null)
                return;

            await tokenRepository.RemoveAsync(refreshToken);            
        }

        public async Task RegisterAsync(RegisterRequest registerRequest)
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
       
        public async Task<AuthenticationResult> RefreshTokenAsync(string refreshToken)
        {
            TokenValidationParameters tokenValidationParameters = new ValidationParametersFactory.ValidationParametersFactory(jwtSettings).RefreshTokenValidationParamters;

            SecurityToken validatedToken = tokenGenerator.ValidateToken(refreshToken, tokenValidationParameters);
            if (validatedToken == null)
                throw new RefreshTokenException("Refresh token invalid");
            
            RefreshToken refToken = await tokenRepository.GetByTokenAsync(refreshToken);

            if (refreshToken == null)
                throw new RefreshTokenException("Refresh token not found");

            User user = await userRepository.GetByIdAsync(refToken.Id);

            string accessToken = await tokenGenerator.GenerateTokenAsync(TokenType.AccessToken, user);

            return new AuthenticationResult()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }

}
