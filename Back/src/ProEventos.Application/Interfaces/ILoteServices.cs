using ProEventos.Application.Dtos;

namespace ProEventos.Application.Interfaces;

public interface ILoteServices
{
    Task AddLoteAsync(int eventoId, LoteDto model);
    Task<LoteDto[]> SaveLoteAsync(int eventoId, LoteDto[] models);
    Task<bool> DeleteLoteAsync(int eventoId, int loteId);
    Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId);
    Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId);
}