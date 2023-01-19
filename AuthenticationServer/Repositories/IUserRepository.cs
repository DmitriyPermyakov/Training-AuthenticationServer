using AuthenticationServer.Models;

namespace AuthenticationServer.Repositories
{
    public interface IUserRepository
    {
        public Task CreateAsync(User user);
        public Task GetById(int id);
        public Task GetAll();
        public Task UpdateAsync(User user);
        public Task DeleteAsync(int id);
    }
}
