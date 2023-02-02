using AuthenticationServer.Models;

namespace AuthenticationServer.Repositories
{
    public interface ITokenRepository
    {
        public Task<RefreshToken> CreateAsync(RefreshToken token);
        public Task<RefreshToken> GetByTokenAsync(string token);
        public Task RemoveAsync(RefreshToken token);
    }
}
