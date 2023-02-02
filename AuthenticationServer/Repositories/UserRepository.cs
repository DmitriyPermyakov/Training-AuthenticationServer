using AuthenticationServer.DTO.Register;
using AuthenticationServer.Models;
using AuthenticationServer.Services;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationServer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthenticationDbContext context;
        private readonly IPasswordHasher _passwordHasher;

        public UserRepository(IPasswordHasher passwordHasher, AuthenticationDbContext context)
        {            
            _passwordHasher = passwordHasher;
            this.context = context;
        }
        public  async Task CreateAsync(RegisterRequest userToCreate)
        {
            if(userToCreate == null)
            {
                throw new ArgumentNullException("userToCreate parameter is null");
            }

            string passwordHash = _passwordHasher.Hash(userToCreate.Password);

            User user = new User
            {
                Name = userToCreate.UserName,
                Email = userToCreate.Email,
                PasswordHash = passwordHash
            };

            await context.Users.AddAsync(user);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            User userFromDb = await context.Users.FirstOrDefaultAsync(user => user.Email == email);
            return userFromDb;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            User user = await context.Users.FirstOrDefaultAsync(user => user.Id == id);
            return user;
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
