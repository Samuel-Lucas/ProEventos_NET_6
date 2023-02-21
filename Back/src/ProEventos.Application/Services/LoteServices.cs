using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Application.Services;

public class LoteServices : ILotePersist
{
    private readonly IGeralPersist _geralPersist;
    private readonly ILotePersist _lotePersist;
    private readonly IMapper _mapper;

    public LoteServices(IGeralPersist geralPersist, ILotePersist lotePersist, IMapper mapper)
    {
        _geralPersist = geralPersist;
        _lotePersist = lotePersist;
        _mapper = mapper;
    }

    public async Task<bool> DeleteLote(int eventoId, int loteId)
    {
        try
        {
            var lote = await _lotePersist.GetLoteByIdsAsync(eventoId, loteId);

            if(lote == null)
                throw new Exception("Lote para delete não encontrado");

            _geralPersist.Delete<Lote>(lote);

            return await _geralPersist.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId)
    {
        try
        {
            var lote = await _lotePersist.GetLoteByIdsAsync(eventoId, loteId);

            if(lote == null) return null!;

            var resultado = _mapper.Map<LoteDto>(lote);
            
            return resultado;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
    {
        try
        {
            var lotes = await _lotePersist.GetLotesByEventoIdAsync(eventoId);

            if(lotes == null) return null!;
            
            var resultado = _mapper.Map<LoteDto[]>(lotes);
            
            return resultado;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}