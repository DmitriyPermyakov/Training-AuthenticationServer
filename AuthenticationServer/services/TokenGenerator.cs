using AuthenticationServer.JwtSettingsParameters;
using AuthenticationServer.Models;
using AuthenticationServer.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationServer.services
{
    enum TokenType
    {
        AccessToken,
        RefreshToken
    }
    public class TokenGenerator : ITokenGenerator
    {
        private readonly JwtSettings jwtSettings;
        private readonly ITokenRepository tokenRepository;

        public TokenGenerator(JwtSettings jwtSettings, ITokenRepository tokenRepository)
        {
            this.jwtSettings = jwtSettings;
            this.tokenRepository = tokenRepository;
        }

        public async Task<string> GenerateToken(TokenType tokenType, User user)
        {
            string tokenSecret = null;
            double expTime = 0;

            switch (tokenType)
            {
                case TokenType.AccessToken:
                    tokenSecret = jwtSettings.AccessTokenSecret;
                    expTime = jwtSettings.AccessTokenExpirationMinutes;
                    break;
                case TokenType.RefreshToken:
                    tokenSecret = jwtSettings.RefreshTokenSecret;
                    expTime = jwtSettings.RefreshTokenExpirationMinutes;
                    break;
            }

            SecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSecret));
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            foreach(var role in user.Roles)
            {
                claims.Add(new Claim("Role", role.ToString()));
            }

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow, 
                expires: DateTime.UtcNow.AddMinutes(expTime),
                signingCredentials
                );

            var createdToken = new JwtSecurityTokenHandler().WriteToken(token);

            if(tokenType == TokenType.RefreshToken)
            {
                RefreshToken refreshToken= new RefreshToken()
                {
                    Token = createdToken,
                    UserId= user.Id
                };
                await tokenRepository.CreateAsync(refreshToken);
            }

            return createdToken;
             
        }

        public SecurityToken ValidateToken(string token, TokenValidationParameters tokenValidationParameters)
        {
            var tokenSecurityHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            try
            {
                ClaimsPrincipal claimsPrincipal = tokenSecurityHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
            }
            catch
            {
                validatedToken = null;
            }

            return validatedToken;
        }
    }
}
