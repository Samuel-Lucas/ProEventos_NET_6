using ProEventos.Domain.Models;

namespace ProEventos.Application.Interfaces
{
    public interface IEventosServices
    {
        Task<Evento> AddEventos(Evento model);
         Task<Evento> UpdateEvento(int eventoId, Evento model);
         Task<bool> DeleteEvento(int eventoId);

        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);
        Task<Evento> GetAllEventosByIdAsync(int eventoId, bool includePalestrantes = false);   
    }
}