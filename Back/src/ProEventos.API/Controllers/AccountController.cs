using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Dtos;
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
        [AllowAnonymous]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _accountServices.GetUserByusernameAsync(userName);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Conta. Erro {ex.Message}");
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                if (await _accountServices.UserExists(userDto.Username))
                    return BadRequest("Usuário já existe !");
                
                var user = await _accountServices.CreateAccountAsync(userDto);
                if (user != null)
                    return Ok(user);
                
                return BadRequest("Usuário não criado, tente novamente mais tarde !");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar registrar usuário. Erro {ex.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await _accountServices.GetUserByusernameAsync(userLogin.Username);
                if (user == null)
                    return Unauthorized("Usuário inválido !");

                var result = await _accountServices.CheckUserPasswordAsync(user, userLogin.Password);
                if (!result.Succeeded)
                    return Unauthorized();
                
                return Ok(new
                {
                    userName = user.Username,
                    primeiroNome = user.PrimeiroNome,
                    token = _tokenServices.CreateToken(user).Result
                });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar realizar login. Erro {ex.Message}");
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _accountServices.GetUserByusernameAsync(User.GetUserName());
                if (user == null) return Unauthorized("Usuário inválido !");
                
                var userReturn = await _accountServices.UpdateAccountAsync(userUpdateDto);
                if (userReturn != null) return NoContent();
                
                return BadRequest("Usuário não criado, tente novamente mais tarde !");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar usuário. Erro {ex.Message}");
            }
        }
    }
}