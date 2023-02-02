using AuthenticationServer.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationServer.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AuthenticationDbContext context;
        public TokenRepository(AuthenticationDbContext context)
        {
            this.context = context;
        }
        public async Task<RefreshToken> CreateAsync(RefreshToken token)
        {
            var createdToken = await context.RefreshTokens.AddAsync(token);
            await context.SaveChangesAsync();

            return createdToken.Entity;
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            var refreshToken = await context.RefreshTokens.Where(t => t.Token == token).FirstOrDefaultAsync();
            return refreshToken;
        }

        public async Task RemoveAsync(RefreshToken token)
        {
            await context.RefreshTokens.Remove(token);
            await context.SaveChangesAsync();
        }
    }
}
