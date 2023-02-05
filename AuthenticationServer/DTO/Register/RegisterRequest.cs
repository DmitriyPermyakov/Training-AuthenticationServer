﻿using System.ComponentModel.DataAnnotations;

namespace AuthenticationServer.DTO.Register
{
    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set;}

    }
}
