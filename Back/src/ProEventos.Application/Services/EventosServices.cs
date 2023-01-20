using ProEventos.Application.Dtos;
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

        public async Task<EventoDto> AddEventos(EventoDto model)
        {
            try
            {
                _geralPersist.Add<EventoDto>(model);

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

        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
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

        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(includePalestrantes);

                if(eventos == null)
                    return null;

                var eventosRetorno = new List<EventoDto>();

                foreach (var evento in eventos)
                {
                    eventosRetorno.Add(new EventoDto() {
                        Id = evento.Id,
                        Local = evento.Local,
                        DataEvento = evento.DataEvento.ToString(),
                        Tema = evento.Tema,
                        QtdPessoas = evento.QtdPessoas,
                        ImagemUrl = evento.ImagemUrl,
                        Telefone = evento.Telefone,
                        Email = evento.Email
                    });
                }
                
                return eventos;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetAllEventosByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventoPersist.GetAllEventosByIdAsync(eventoId, includePalestrantes);

                if(evento == null)
                    return null;
                
                return evento;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventoPorTema = await _eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);

                if(eventoPorTema == null)
                    return null;
                
                return eventoPorTema;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}