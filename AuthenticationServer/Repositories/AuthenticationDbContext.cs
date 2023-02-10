using Microsoft.EntityFrameworkCore;
using AuthenticationServer.Models;
using AuthenticationServer.Converters;

namespace AuthenticationServer.Repositories
{
    public class AuthenticationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new EnumCollectionJsonValueConverter<Role>();
            var comparer = new CollectionValueComparer<Role>();

            modelBuilder.Entity<User>()
                .Property(u => u.Roles)
                .HasConversion(converter)
                .Metadata.SetValueComparer(comparer);
        }

    }

    
}
