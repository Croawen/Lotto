using Lotto.Api.Filters;
using Lotto.Services.AuthService;
using Lotto.Services.AuthService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lotto.Api.Controllers
{
    [Route("auth")]
    [ApiExceptionFilter]
    public class AuthController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
        {
            await _authService.Register(model);
            return new OkResult();
        }

        [HttpPost("login")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LoginResultModel), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            var res = await _authService.Login(model);
            return new OkObjectResult(res);
        }
    }
}
