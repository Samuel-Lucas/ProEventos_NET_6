using ProEventos.Application.Dtos;

namespace ProEventos.Application.Interfaces
{
    public interface IEventosServices
    {
        Task<EventoDto> AddEvento(int idUsuario, EventoDto model);
        Task<EventoDto> UpdateEvento(int idUsuario, int eventoId, EventoDto model);
        Task<bool> DeleteEvento(int idUsuario, int eventoId);

        Task<EventoDto[]> GetAllEventosByTemaAsync(int idUsuario, string tema, bool includePalestrantes = false);
        Task<EventoDto[]> GetAllEventosAsync(int idUsuario, bool includePalestrantes = false);
        Task<EventoDto> GetEventoByIdAsync(int idUsuario, int eventoId, bool includePalestrantes = false);   
    }
}