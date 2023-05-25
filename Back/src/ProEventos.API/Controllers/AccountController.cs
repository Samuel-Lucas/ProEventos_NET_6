using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Interfaces;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices _accountServices;
        private readonly ITokenServices _tokenServices;

        public AccountController(IAccountServices accountServices, ITokenServices tokenServices)
        {
            _accountServices = accountServices;
            _tokenServices = tokenServices;
        }

        [HttpGet("GetUser/{userName}")]
        public async Task<IActionResult> GetUser(string userName)
        {
            try
            {
                var user = await _accountServices.GetUserByusernameAsync(userName);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Conta. Erro {ex.Message}");
            }
        }
    }
}