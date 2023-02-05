using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthenticationServer.JwtSettingsParameters;

namespace AuthenticationServer.ValidationParametersFactory
{
    public class ValidationParametersFactory
    {
        private readonly JwtSettings jwtSettings;
        public ValidationParametersFactory(JwtSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
        }
     
        public TokenValidationParameters AccessTokenValidationParameters
        {
            get
            {
                return new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = false,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.AccessTokenSecret))
                };
            }
        }

        public TokenValidationParameters RefreshTokenValidationParamters
        {
            get
            {
                return new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = false,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.RefreshTokenSecret))
                };
            }
        }
    }
}
