using ProEventos.Domain.Models;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ProEventos.Persistence;

public class LotePersist : ILotePersist
{
    private readonly ProEventosContexto _context;

    public LotePersist(ProEventosContexto context)
    {
        _context = context;       
    }

    public async Task<Lote> GetLoteByIdsAsync(int eventoId, int id)
    {
        IQueryable<Lote> query = _context.Lotes;
        query = query
                .AsNoTracking()
                .Where(lote => lote.EventoId == eventoId && lote.Id == id);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId)
    {
        IQueryable<Lote> query = _context.Lotes;
        query = query
                .AsNoTracking()
                .Where(lote => lote.EventoId == eventoId);

        return await query.ToArrayAsync();
    }
}