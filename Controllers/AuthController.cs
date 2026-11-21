using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RefundManager.DTOs;
using RefundManager.Interfaces;

namespace RefundManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginOutputDto>> Login([FromBody] LoginInputDto loginInputDto)
        {
            var result = await _authService.LoginAsync(loginInputDto.Name, loginInputDto.Surname, loginInputDto.Password);

            if (result == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(result);
        }
    }
}
