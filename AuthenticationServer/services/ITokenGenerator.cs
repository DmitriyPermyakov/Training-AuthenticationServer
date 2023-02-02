using AuthenticationServer.Models;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationServer.services
{
    public interface ITokenGenerator
    {
        public Task<string> GenerateToken(TokenType tokenType, User user);
        public SecurityToken ValidateToken(string token, TokenValidationParameters tokenValidationParameters);
    }
}
