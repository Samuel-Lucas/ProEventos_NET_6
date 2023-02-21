using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Interfaces;

public interface ILotePersist
{
    /// <summary>
    /// Método que retornará uma lista de lotes por eventoId
    /// </summary>
    /// <param name="eventoId">Código chave da tabela evento</param>
    /// <returns>Lista de lotes</returns>
    Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);

    /// <summary>
    /// Método que retornará apenas 1 lote
    /// </summary>
    /// <param name="eventoId">Código chave da tabela evento</param>
    /// <param name="id">Código chave da tabela lote</param>
    /// <returns>Apenas um lote</returns>
    Task<Lote> GetLoteByIdsAsync(int eventoId, int id);
}