using AuthenticationServer.DTO.Register;
using AuthenticationServer.Models;

namespace AuthenticationServer.Repositories
{
    public interface IUserRepository
    {
        public Task CreateAsync(RegisterRequest userToCreate);
        public Task GetByIdAsync(int id);
        public Task<User> GetByEmailAsync(string email);
        public Task GetAllAsync();
        public Task UpdateAsync(User user);
        public Task DeleteAsync(int id);
    }
}
