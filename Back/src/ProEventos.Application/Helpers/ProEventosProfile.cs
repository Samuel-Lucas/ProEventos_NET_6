using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Helpers
{
    public class ProEventosProfile : Profile
    {
        public ProEventosProfile()
        {
            CreateMap<Evento, EventoDto>();
        }
    }
}