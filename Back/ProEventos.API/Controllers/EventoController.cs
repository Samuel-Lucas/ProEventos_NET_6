using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{
    public IEnumerable<Evento> _evento = new Evento[] {
            new Evento {
                EventoId = 1,
                Tema = "Angular",
                Local = "SP",
                Lote = "1º lote",
                QtdPessoas = 120,
                DataEvento = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy"),
                ImagemUrl = "foto.png"
            },
            new Evento {
                EventoId = 2,
                Tema = "Angular e novidades",
                Local = "BH",
                Lote = "2º lote",
                QtdPessoas = 220,
                DataEvento = DateTime.Now.AddDays(3).ToString("dd/MM/yyyy"),
                ImagemUrl = "foto.png"
            },
    };

    private readonly ILogger<EventoController> _logger;

    public EventoController()
    {       
    }

    [HttpGet]
    public IEnumerable<Evento> Get()
    {
        return _evento;
    }

    [HttpGet("{id}")]
    public IEnumerable<Evento> GetById(int id)
    {
        return _evento.Where(evento => evento.EventoId == id);
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