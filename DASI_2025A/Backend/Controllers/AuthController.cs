using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Dto;

namespace Backend.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth) 
        {
            _auth = auth;    
        }

        // Registro de un usuario
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                var response = await _auth.RegisterAsync(registerDto);
                return Created(string.Empty, response);

            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error del servidor al registrar al usuario {ex.Message}");
            }
        }

        // Login de un usuario
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var response = await _auth.LoginAsync(loginDto);
                return Created(string.Empty, response);

            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error del servidor al hacer login {ex.Message}");
            }
        }
    }
}
