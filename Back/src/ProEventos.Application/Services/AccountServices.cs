using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Application.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserPersist _userPersist;

        public AccountServices(UserManager<User> userManager,
                               SignInManager<User> signInManager,
                               IMapper mapper,
                               IUserPersist userPersist)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userPersist = userPersist;
        }

        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                var user = await _userManager.Users
                                            .SingleOrDefaultAsync(user => user.UserName == userUpdateDto.Username.ToLower());
                
                return await _signInManager.CheckPasswordSignInAsync(user!, password, false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar Password. Error: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> CreateAccountAsync(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                {
                    var userToReturn = _mapper.Map<UserUpdateDto>(user);
                    return userToReturn;
                }

                return null!;
            }
            catch (Exception ex)
            {
                var msg = $"Erro ao tentar criar conta. Error: {ex.Message} {ex.InnerException}";
                var inner = ex.InnerException;
                throw new Exception(msg);
            }
        }

        public async Task<UserUpdateDto> GetUserByusernameAsync(string username)
        {
            try
            {
                var user = await _userPersist.GetUserByUsernameAsync(username);
                
                if (user == null) return null!;

                var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
                return userUpdateDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar consultr usuário. Error: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> UpdateAccountAsync(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userPersist.GetUserByUsernameAsync(userUpdateDto.Username);

                if (user == null) return null!;

                _mapper.Map(userUpdateDto, user);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);

                _userPersist.Update<User>(user);

                if (await _userPersist.SaveChangesAsync())
                {
                    var userRetorno = await _userPersist.GetUserByUsernameAsync(user.UserName);
                    return _mapper.Map<UserUpdateDto>(userRetorno);
                }

                return null!;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar atualizar a conta. Error: {ex.Message}");
            }
        }

        public async Task<bool> UserExists(string username)
        {
            try
            {
                return await _userManager.Users
                                        .AnyAsync(user => user.UserName == username.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar se usuário existe. Error: {ex.Message}");
            }
        }
    }
}