using ProEventos.Application.Dtos;

namespace ProEventos.Application.Interfaces
{
    public interface ITokenServices
    {
        Task<string> CreateToken(UserUpdateDto userUpdateDto);
    }
}