using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Models;
using ProEventos.API.Persistence;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private EventoDbContext _context;
    private readonly ILogger<EventosController> _logger;

    public EventosController(EventoDbContext context)
    {
        _context = context;       
    }

    [HttpGet]
    public IEnumerable<Evento> Get()
    {
        return _context.Eventos;
    }

    [HttpGet("{id}")]
    public Evento GetById(int id)
    {
        return _context.Eventos.FirstOrDefault(evento => evento.EventoId == id);
    }

    [HttpPost]
    public string Post()
    {
        return "post";
    }

    [HttpPut("{id}")]
    public string Put()
    {
        return "put";
    }

    [HttpDelete("{id}")]
    public string Delete()
    {
        return "delete";
    }
}