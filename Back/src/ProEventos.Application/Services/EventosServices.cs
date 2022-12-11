using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Application.Services
{
    public class EventosServices : IEventosServices
    {
        private readonly IGeralPersist _geralPersist;
        private readonly IEventoPersist _eventoPersist;

        public EventosServices(IGeralPersist geralPersist, IEventoPersist eventoPersist)
        {
            _geralPersist = geralPersist;
            _eventoPersist = eventoPersist;
        }

        public async Task<Evento> AddEventos(Evento model)
        {
            try
            {
                _geralPersist.Add<Evento>(model);

                if(await _geralPersist.SaveChangesAsync())
                {
                    return await _eventoPersist.GetAllEventosByIdAsync(model.Id, false);
                }

                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> UpdateEvento(int eventoId, Evento model)
        {
            try
            {
                var evento = await _eventoPersist.GetAllEventosByIdAsync(eventoId, false);

                if(evento == null)
                    return null;

                model.Id = evento.Id;

                _geralPersist.Update<Evento>(model);

                if(await _geralPersist.SaveChangesAsync())
                {
                    return await _eventoPersist.GetAllEventosByIdAsync(model.Id, false);
                }

                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await _eventoPersist.GetAllEventosByIdAsync(eventoId, false);

                if(evento == null)
                    throw new Exception("Evneto para delete não encontrado");

                _geralPersist.Delete<Evento>(evento);

                return await _geralPersist.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            return default;
        }

        public Task<Evento> GetAllEventosByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            return default;
        }

        public Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            return default;
        }
    }
}