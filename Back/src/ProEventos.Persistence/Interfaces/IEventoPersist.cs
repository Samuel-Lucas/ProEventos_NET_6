using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Interfaces
{
    public interface IEventoPersist
    {
        Task<Evento[]> GetAllEventosByTemaAsync(int idUsuario, string tema, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosAsync(int idUsuario, bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsync(int idUsuario, int eventoId, bool includePalestrantes = false);
    }
}