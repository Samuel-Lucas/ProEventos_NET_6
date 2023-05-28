using AutoMapper;
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
        private readonly IMapper _mapper;

        public EventosServices(IGeralPersist geralPersist, IEventoPersist eventoPersist, IMapper mapper)
        {
            _geralPersist = geralPersist;
            _eventoPersist = eventoPersist;
            _mapper = mapper;
        }

        public async Task<EventoDto> AddEvento(int idUsuario, EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                evento.UserId = idUsuario;
            
                _geralPersist.Add<Evento>(evento);

                if(await _geralPersist.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(evento.UserId, evento.Id, false);
                    return _mapper.Map<EventoDto>(eventoRetorno);
                }

                return null!;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int idUsuario, int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(idUsuario, eventoId, false);

                if(evento == null)
                    return null!;

                model.Id = evento.Id;
                model.UserId = idUsuario;

                _mapper.Map(model, evento);

                _geralPersist.Update<Evento>(evento);

                if(await _geralPersist.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(idUsuario, evento.Id, false);
                    return _mapper.Map<EventoDto>(eventoRetorno);
                }

                return null!;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int idUsuario, int eventoId)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(idUsuario, eventoId, false);

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

        public async Task<EventoDto[]> GetAllEventosAsync(int idUsuario, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(idUsuario, includePalestrantes);

                if(eventos == null)
                    return null!;
                
                var resultado = _mapper.Map<EventoDto[]>(eventos);
                
                return resultado;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoByIdAsync(int idUsuario, int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(idUsuario, eventoId, includePalestrantes);

                if(evento == null) return null!;

                var resultado = _mapper.Map<EventoDto>(evento);
                
                return resultado;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(int idUsuario, string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventosPorTema = await _eventoPersist.GetAllEventosByTemaAsync(idUsuario, tema, includePalestrantes);

                if(eventosPorTema == null) return null!;
                
                var resultado = _mapper.Map<EventoDto[]>(eventosPorTema);
                
                return resultado;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}