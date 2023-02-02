using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using AuthenticationServer.DTO.Register;
using AuthenticationServer.Repositories;
using AuthenticationServer.Models;
using AuthenticationServer.DTO.Login;
using AuthenticationServer.Services;
using AuthenticationServer.Exceptions;
using AuthenticationServer.services;

namespace AuthenticationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
       
        private readonly IAccountService accountService;
        public AccountController(IUserRepository userRepo, IPasswordHasher passwordHasher, IAccountService accountService)
        {            
            this.accountService = accountService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(registerRequest);

                await accountService.Register(registerRequest);

                return Ok("User successfully registered");
            }
            catch (RegisterException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if(!ModelState.IsValid) 
                    return BadRequest(loginRequest);

               


            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {

        }       

    }
}
