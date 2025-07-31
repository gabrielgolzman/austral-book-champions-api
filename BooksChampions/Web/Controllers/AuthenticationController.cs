using Application.Interfaces;
using Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ICustomAuthenticationService _customAuthenticationService;

        public AuthenticationController(IConfiguration config, ICustomAuthenticationService autenticacionService)
        {
            _config = config;
            _customAuthenticationService = autenticacionService;
        }
        [HttpPost] 
        public ActionResult<string> Login(LoginRequest loginRequest) 
        {
            var token = _customAuthenticationService.Login(loginRequest); 
            if(string.IsNullOrEmpty(token))
                return StatusCode(401);

            return Ok(token);
        }

    }
}
