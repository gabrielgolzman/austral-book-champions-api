using Application.Interfaces;
using Application.Models.Requests;
using Application.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ICustomAuthenticationService _customAuthenticationService;

        public AuthenticationController(ICustomAuthenticationService authenticationService)
        {
            _customAuthenticationService = authenticationService;
        }

        [HttpPost("login")] 
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest loginRequest) 
        {
            // Check if the user is already authenticated (not sure if necessary, evaluate later)
            // if (User.Identity?.IsAuthenticated == true)
            //     return BadRequest(new AuthResponse { Success = false, Message = "User is already logged in" });

            var token = await _customAuthenticationService.Login(loginRequest); 
            if(string.IsNullOrEmpty(token))
                return StatusCode(401, new AuthResponse { Success = false, Message = "Invalid credentials" });

            return Ok(new AuthResponse { Success = true, Message = "Login successful", Token = token });
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest registerRequest)
        {
            // Check if the user is already authenticated (not sure if necessary, evaluate later)
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return BadRequest(new AuthResponse { Success = false, Message = "User is already logged in" });

            var success = await _customAuthenticationService.Register(registerRequest);
            if (!success)
                return BadRequest(new AuthResponse { Success = false, Message = "User already exists or invalid data" });

            return Ok(new AuthResponse { Success = true, Message = "User registered successfully" });
        }

    }
}
