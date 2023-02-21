using ProEventos.Application.Dtos;

namespace ProEventos.Application.Interfaces;

public interface ILoteServices
{
    Task<LoteDto> SaveLote(int loteId, LoteDto[] models);
    Task<bool> DeleteLote(int eventoId, int loteId);
    Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId);
    Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId);
}