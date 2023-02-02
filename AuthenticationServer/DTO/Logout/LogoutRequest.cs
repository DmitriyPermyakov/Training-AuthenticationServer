using System.ComponentModel.DataAnnotations;

namespace AuthenticationServer.DTO.Logout
{
    public class LogoutRequest
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
