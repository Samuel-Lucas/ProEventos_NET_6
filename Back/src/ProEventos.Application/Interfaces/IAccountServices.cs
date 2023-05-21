using ProEventos.Application.Dtos;
using Microsoft.AspNetCore.Identity;

namespace ProEventos.Application.Interfaces
{
    public interface IAccountServices
    {
        Task<bool> UserExists(string username);
        Task<UserUpdateDto> GetUserByusernameAsync(string username);
        Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password);
        Task<UserDto> CreateAccountAsync(UserDto userDto);
        Task<UserUpdateDto> UpdateAccountAsync(UserUpdateDto userUpdateDto);
    }
}