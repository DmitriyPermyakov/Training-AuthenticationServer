using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using AuthenticationServer.DTO.Register;
using AuthenticationServer.Repositories;
using AuthenticationServer.Models;
using AuthenticationServer.DTO.Login;
using AuthenticationServer.Services;
using AuthenticationServer.Exceptions;
using AuthenticationServer.services;
using AuthenticationServer.DTO;
using AuthenticationServer.DTO.Logout;
using Microsoft.AspNetCore.Authorization;

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

                await accountService.RegisterAsync(registerRequest);

                return Ok("User successfully registered");
            }
            catch (RegisterException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if(!ModelState.IsValid) 
                    return BadRequest(loginRequest);
                
                AuthenticationResult response = await accountService.LoginAsync(loginRequest);
                return Ok(response);
            }
            catch (LoginException ex)
            {
                return Unauthorized(ex.Message);                
            }

        }
        [Authorize()]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout(LogoutRequest logoutRequest)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(logoutRequest);
                if (logoutRequest == null)
                    return BadRequest("logout request is null");

                await accountService.Logout(logoutRequest);
                return Unauthorized("Successfully logout");
            }
            catch(LogoutException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(refreshToken))
                    return Unauthorized("Refresh token is empty");

                AuthenticationResult result = await accountService.RefreshTokenAsync(refreshToken);

                return Ok(result);
            }
            catch(RefreshTokenException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

    }
}
