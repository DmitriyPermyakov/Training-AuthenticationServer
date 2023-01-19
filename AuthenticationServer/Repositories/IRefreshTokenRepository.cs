using AuthenticationServer.Models;

namespace AuthenticationServer.Repositories
{
    public interface IRefreshTokenRepository
    {
        public Task CreateAsync(RefreshToken refreshToken);
        public Task RemoveAsync(string refreshToken);
        public Task GetByTokenAsync(string refreshToken);
    }
}
