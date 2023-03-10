namespace AuthenticationServer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Role> Roles { get; set; }
    }

    public enum Role
    {
        Admin,
        User
    }
}
