using Microsoft.EntityFrameworkCore;
using ProEventos.API.Models;

namespace ProEventos.API.Persistence
{
    public class EventoDbContext : DbContext
    {
        public EventoDbContext(DbContextOptions<EventoDbContext> options) : base(options)
        {
        }

        public DbSet<Evento> Eventos { get; set; }
    }
}