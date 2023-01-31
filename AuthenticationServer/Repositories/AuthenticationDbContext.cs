using Microsoft.EntityFrameworkCore;
using AuthenticationServer.Models;

namespace AuthenticationServer.Repositories
{
    public class AuthenticationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options) { }

    }
}
